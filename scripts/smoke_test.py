"""HTTP checks for the running Lab 2 prototype. Python is a test tool, not an app dependency."""
import sys
from urllib.request import urlopen, Request
from urllib.parse import urlencode, urlsplit
from urllib.error import HTTPError
from html.parser import HTMLParser

base = sys.argv[1] if len(sys.argv) > 1 else 'http://127.0.0.1:5080'
checks = 0

def request(path, data=None):
    req = Request(base + path, data=urlencode(data).encode() if data is not None else None)
    try:
        with urlopen(req, timeout=10) as r:
            return r.status, r.read().decode()
    except HTTPError as e:
        return e.code, e.read().decode()

def check(condition, message):
    global checks
    assert condition, message
    checks += 1

class Links(HTMLParser):
    def __init__(self): super().__init__(); self.urls = []
    def handle_starttag(self, tag, attrs):
        if tag in ['a', 'link']:
            for k,v in attrs:
                if k == 'href' and v and v.startswith('/'):
                    self.urls.append(v.split('#')[0])

# Crawl actual rendered links, including every demonstration entity's details.
queue = ['/']; seen = set()
while queue:
    path = queue.pop(0)
    if path in seen: continue
    seen.add(path)
    status, body = request(path)
    check(status == 200, f'GET {path}: {status}')
    if '.css' not in path:
        check('QuTech' in body and 'DOCTYPE html' in body, f'Incomplete view: {path}')
        parser = Links(); parser.feed(body)
        queue.extend(x for x in parser.urls if x not in seen)

for controller in ['Customers','Plans','Modules','Subscriptions','Usage','Invoices','Administrators']:
    check(request(f'/{controller}/Details/99999')[0] == 404, f'{controller} missing ID')
    status, body = request(f'/{controller}/Create', {})
    check(status == 200 and 'Please correct the highlighted fields' in body and 'Validation passed.' not in body, f'{controller} empty form')

valid = {
 'Customers': {'FullName':'Case Test','CompanyName':'Test Company','Email':'case@example.com','Phone':'+61 2 5550 0104'},
 'Administrators': {'FullName':'Test Admin','Email':'admin@example.com','Phone':'+61 2 5550 0105','Department':'Support'},
 'Plans': {'Name':'Test Plan','Description':'For test teams','MonthlyPrice':'10.50','IncludedApiCalls':'1000'},
 'Modules': {'Name':'Test Module','Description':'A test capability'},
 'Subscriptions': {'CustomerId':'1','PlanId':'2','StartDate':'2026-10-08','Status':'Active'},
 'Usage': {'SubscriptionId':'1','PeriodStart':'2026-10-01','ApiCalls':'250'},
 'Invoices': {'Reference':'TEST-01','SubscriptionId':'1','IssuedOn':'2026-10-01','DueOn':'2026-10-15','Amount':'149.00','Status':'Draft'},
 'Configuration': {'PlanId':'1','ModuleId':'2','Enabled':'true'}
}
for controller, data in valid.items():
    before = request(f'/{controller}')[1]
    status, body = request(f'/{controller}/Create', data)
    check(status == 200 and 'Validation passed.' in body, f'{controller} valid preview')
    after = request(f'/{controller}')[1]
    check(before == after, f'{controller} must not persist previews')

bad = [
 ('Customers','Email','bad','Enter an email address'),
 ('Plans','MonthlyPrice','-1','price of zero or more'),
 ('Plans','IncludedApiCalls','0','at least one API call'),
 ('Plans','MonthlyPrice','not-a-number','price of zero or more'),
 ('Subscriptions','PlanId','99999','existing subscription plan'),
 ('Subscriptions','CustomerId','99999','existing customer'),
 ('Subscriptions','StartDate','invalid-date','valid subscription start date'),
 ('Subscriptions','Status','Unknown','Choose Active, Trial or Paused'),
 ('Invoices','DueOn','2026-09-01','on or after the issue date'),
 ('Invoices','Amount','-10','amount of zero or more'),
 ('Usage','ApiCalls','-1','zero or more'),
 ('Usage','SubscriptionId','99999','existing subscription'),
 ('Configuration','ModuleId','99999','existing module'),
 ('Configuration','Enabled','invalid','valid enabled setting')
]
for controller,field,value,message in bad:
    data = dict(valid[controller]); data[field] = value
    status, body = request(f'/{controller}/Create', data)
    check(status == 200 and message in body and 'Validation passed.' not in body, f'{controller}: {field}={value}')
    check('is not valid for' not in body and "The value '" not in body, f'{controller}: framework default message leaked')

status,body=request('/BillingProfiles/Edit/1', {'BillingName':'Changed Example','BillingEmail':'billing@example.com','Address':'A fictional address'})
check(status == 200 and 'Validation passed.' in body, 'Billing profile preview')
check('Changed Example' not in request('/Customers/Details/1')[1], 'Billing preview must not persist')
check(request('/BillingProfiles/Edit/99999')[0]==404, 'Missing billing profile')
status,body=request('/BillingProfiles/Edit/1', {})
check(status==200 and 'Enter the billing organisation name.' in body, 'Billing profile required fields')

# Razor must encode untrusted display strings.
xss=dict(valid['Customers']); xss['CompanyName']='<script>alert(1)</script>'
status,body=request('/Customers/Create',xss)
check('&lt;script&gt;' in body and '<script>alert(1)</script>' not in body, 'Razor output encoding')
status,home=request('/')
check('548' in home and '399' in home, 'Dashboard sample totals')
check('182,200' in request('/Home/Analytics')[1], 'API usage total')
print(f'PASS: {checks} assertions; {len(seen)} linked GET routes/assets; valid previews, invalid input, missing IDs, non-persistence, encoding and totals.')
