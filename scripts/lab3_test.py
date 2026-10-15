#!/usr/bin/env python3
"""HTTP integration evidence using a disposable SQLite database; never touches submission data."""
import argparse, http.cookiejar, json, os, socket, sqlite3, subprocess, time, traceback
from pathlib import Path
from urllib import request, parse, error
from html.parser import HTMLParser

ROOT = Path(__file__).resolve().parents[1]
APP = ROOT / 'JustinLee-Lab2'
OUT = ROOT / 'artifacts/tests'
DB = OUT / 'lab3-test.db'
BASE = 'http://127.0.0.1:5086'
RESULTS = []
PROCESS = None
SERVER_LOG = None
class Tokens(HTMLParser):
    def __init__(self, html):
        super().__init__(); self.token = None; self.feed(html)
    def handle_starttag(self, tag, attrs):
        a = dict(attrs)
        if tag == 'input' and a.get('name') == '__RequestVerificationToken': self.token = a.get('value')
class Client:
    def __init__(self): self.opener = request.build_opener(request.HTTPCookieProcessor(http.cookiejar.CookieJar()))
    def get(self, path): return self.send(path)
    def send(self, path, data=None):
        req = request.Request(BASE+path, data=None if data is None else parse.urlencode(data).encode())
        try: response = self.opener.open(req, timeout=20)
        except error.HTTPError as exc: response = exc
        return response.status, response.read().decode('utf-8', errors='replace'), response.url
    def post(self, path, data, token_path=None):
        status, html, _ = self.get(token_path or path)
        token = Tokens(html).token
        assert token, f'No CSRF token in {token_path or path}: HTTP {status}'
        return self.send(path, dict(data, __RequestVerificationToken=token))
def sql(statement, args=()):
    with sqlite3.connect(DB) as db: return db.execute(statement,args).fetchall()
def scalar(statement,args=()): return sql(statement,args)[0][0]
def check(condition, text):
    if not condition: raise AssertionError(text)
def test(title, steps, expected, fn):
    item={'id':f'TC-{len(RESULTS)+1:02}', 'title':title,'steps':steps,'expected':expected}
    try:
        detail=fn(); item.update(status='PASS', actual=str(detail or expected))
    except Exception as exc:
        item.update(status='FAIL', actual=f'{type(exc).__name__}: {exc}')
    RESULTS.append(item); print(f"{item['id']} {item['status']} {title}: {item['actual']}",flush=True)
    (OUT/'test-results.json').write_text(json.dumps(RESULTS,indent=2))
def stop():
    global PROCESS
    if PROCESS:
        PROCESS.terminate()
        try: PROCESS.wait(timeout=10)
        except subprocess.TimeoutExpired: PROCESS.kill(); PROCESS.wait()
        PROCESS=None

def start():
    global PROCESS,SERVER_LOG
    env=dict(os.environ, ASPNETCORE_ENVIRONMENT='Development', DOTNET_USE_POLLING_FILE_WATCHER='1', DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE='false', ConnectionStrings__QuTech='Data Source='+str(DB))
    SERVER_LOG=open(OUT/'test-server.log','a')
    PROCESS=subprocess.Popen(['dotnet',str(APP/'bin/Debug/net10.0/JustinLee-Lab2.dll'),'--urls',BASE],cwd=APP,env=env,stdout=SERVER_LOG,stderr=subprocess.STDOUT)
    for _ in range(120):
        if PROCESS.poll() is not None: raise RuntimeError('Test server stopped; inspect test-server.log')
        try:
            if Client().get('/Account/Login')[0]==200:return
        except (OSError,error.URLError): pass
        time.sleep(.25)
    raise RuntimeError('Test server readiness timeout')
def login(client,email='alex@example.com',password='QuTechLab3!2026'):
    status,html,url=client.post('/Account/Login',{'Email':email,'Password':password})
    check(status==200 and '/Account/Login' not in url,'Login did not reach dashboard')
    return status,html,url

def run():
    client=Client(); anon=Client(); ids={}
    test('Anonymous access restricted','GET /Customers without cookies','Redirect to sign-in', lambda: check('/Account/Login' in anon.get('/Customers')[2], 'Anonymous customer access was not challenged'))
    test('Sign-in rejects invalid credentials','POST wrong password for a non-existing account','Generic custom error; no login',lambda:check('Unable to sign in.' in anon.post('/Account/Login',{'Email':'absent@example.com','Password':'Wrong!123'})[1],'Missing generic error'))
    test('Login required fields','POST empty login form','Custom email and password errors',lambda:check('Enter your administrator email' in anon.post('/Account/Login',{})[1],'Missing required error'))
    test('External redirect rejected','GET login with ReturnUrl=https://example.com','HTTP 400',lambda:check(anon.get('/Account/Login?ReturnUrl=https%3A%2F%2Fexample.com')[0]==400,'External ReturnUrl not rejected'))
    test('Login CSRF protection','POST login without antiforgery token','HTTP 400',lambda:check(anon.send('/Account/Login',{'Email':'alex@example.com','Password':'QuTechLab3!2026'})[0]==400,'Missing login CSRF rejection'))
    test('Successful administrator sign-in','Use seeded Alex credentials','Dashboard and login audit',lambda: login(client) and check(scalar("SELECT count(*) FROM AuditEntries WHERE Action='Administrator signed in.'")>0,'Missing login audit'))
    def seeds():
        counts={t:scalar('SELECT count(*) FROM '+t) for t in ['Plans','Modules','Subscriptions','UsageRecords','Invoices','Discounts','BillingProfiles','AuditEntries','PlanModules','AspNetUsers']}
        counts['Customers']=scalar("SELECT count(*) FROM People WHERE PersonType='Customer'")
        counts['Administrators']=scalar("SELECT count(*) FROM People WHERE PersonType='Administrator'")
        check(all(n>=5 for n in counts.values()),str(counts)); return counts
    test('Seed and relationship data','Inspect fresh migrated SQLite tables','At least five rows per major entity',seeds)
    def menus():
        paths=['/','/Customers','/Administrators','/Plans','/Modules','/Discounts','/Configuration','/Subscriptions','/Usage','/Invoices','/Home/Analytics','/Audit','/Home/About']
        for path in paths: check(client.get(path)[0]==200,'Failed '+path)
        return str(len(paths))+' menu routes returned HTTP 200'
    test('All application menus','GET every navigation destination','All pages render for authenticated administrator',menus)
    test('Write CSRF protection','POST plan without token','HTTP 400 and no write',lambda:check(client.send('/Plans/Create',{'Name':'NoToken'})[0]==400,'Missing write CSRF rejection'))
    specs={
        'Customers':('People',{'FullName':'Test Customer','CompanyName':'Test Studio','Email':'integration@example.com','Phone':'+61 2 5550 0900','billingAddress':'1 Test Lane Sydney'},'Email','integration@example.com'),
        'Plans':('Plans',{'Name':'Integration Plan','Description':'Integration monthly plan','MonthlyPrice':'100','IncludedApiCalls':'10000'},'Name','Integration Plan'),
        'Modules':('Modules',{'Name':'Integration Module','Description':'Integration module description'},'Name','Integration Module'),
        'Discounts':('Discounts',{'Name':'Integration Discount','Description':'Integration discount description','Percentage':'10','StartsOn':'2026-01-01','EndsOn':'2027-12-31','Enabled':'true'},'Name','Integration Discount'),
    }
    def create(route):
        table,data,key,value=specs[route]; before=scalar('SELECT count(*) FROM '+table)
        s,h,u=client.post('/'+route+'/Create',data)
        check(s==200 and '/Details/' in u, 'Create did not redirect to Details: '+u+' '+h[-400:])
        check(scalar('SELECT count(*) FROM '+table)==before+1,'Row not persisted')
        ids[route]=scalar('SELECT Id FROM '+table+' WHERE '+key+'=?',(value,)); return 'Persisted record '+str(ids[route])
    for route in list(specs):
        test(route+' required validation','POST empty Create form','Validation errors and unchanged row count',lambda r=route: invalid_empty(client,r,specs[r][0]))
        test(route+' create','Submit complete unique form','New record stored and details displayed',lambda r=route:create(r))
    specs['Subscriptions']=('Subscriptions',{'CustomerId':ids.get('Customers',1),'PlanId':ids.get('Plans',1),'DiscountId':ids.get('Discounts',1),'StartDate':'2026-09-01','Status':'Active'},'CustomerId',ids.get('Customers',1))
    test('Subscription create','Create subscription linking new customer, plan and discount','Related subscription persisted',lambda:create('Subscriptions'))
    specs['Usage']=('UsageRecords',{'SubscriptionId':ids.get('Subscriptions',1),'PeriodStart':'2026-11-01','ApiCalls':'500'},'SubscriptionId',ids.get('Subscriptions',1))
    test('Usage create','Submit usage linked to new subscription','Usage persisted',lambda:create('Usage'))
    for route in ['Subscriptions','Usage']:
        test(route+' required validation','POST empty Create form','Custom validation and unchanged DB',lambda r=route:invalid_empty(client,r,specs[r][0]))
    def edit(route):
        table,data,_,_=specs[route]; data=dict(data,Id=ids[route])
        field,value={'Customers':('CompanyName','Updated Test Studio'),'Plans':('MonthlyPrice','120'),'Modules':('Description','Updated module'),'Discounts':('Percentage','15'),'Subscriptions':('Status','Paused'),'Usage':('ApiCalls','700')}[route]
        data[field]=value
        s,h,u=client.post('/'+route+'/Edit/'+str(ids[route]),data)
        check(s==200 and '/Details/' in u,'Edit failed '+u)
        actual=scalar('SELECT '+field+' FROM '+table+' WHERE Id=?',(ids[route],))
        check((float(actual)==float(value)) if field in ['MonthlyPrice','Percentage','ApiCalls'] else str(actual)==value,'Edited value not saved: '+str(actual)); specs[route]=(table,{k:v for k,v in data.items() if k!='Id'},specs[route][2],specs[route][3])
    for route in specs: test(route+' edit','Update one field through edit form','Updated value persisted',lambda r=route:edit(r))
    def restart():
        stop(); start(); login(client)
        check(scalar('SELECT CompanyName FROM People WHERE Id=?',(ids['Customers'],))=='Updated Test Studio','Value lost after restart')
        check('Updated Test Studio' in client.get('/Customers/Details/'+str(ids['Customers']))[1],'UI lost record after restart')
    test('Persistence across server restart','Stop process, restart with same DB, sign in','Created and edited record retained',restart)
    test('Linked customer deletion blocked','Try deleting customer with subscription','Customer retained; custom in-use error',lambda:check('in use' in client.post('/Customers/Delete/'+str(ids['Customers']),{'id':ids['Customers']})[1] and scalar('SELECT count(*) FROM People WHERE Id=?',(ids['Customers'],))==1,'Linked deletion not protected'))
    def billing():
        cid=ids['Customers']; client.post('/BillingProfiles/Edit/'+str(cid),{'BillingName':'Integration Billing','BillingEmail':'billing@example.com','Address':'2 New Lane Sydney'})
        check(scalar('SELECT Address FROM BillingProfiles WHERE CustomerId=?',(cid,))=='2 New Lane Sydney','Billing profile not saved')
    test('One-to-one billing profile update','Edit billing profile for created customer','Shared customer key retained and address changed',billing)
    def config():
        p,m=ids['Plans'],ids['Modules']; client.post('/Configuration/Create',{'PlanId':p,'ModuleId':m,'Enabled':'true'})
        check(scalar('SELECT Enabled FROM PlanModules WHERE PlanId=? AND ModuleId=?',(p,m))==1,'Join row not created')
        client.post('/Configuration/Create',{'PlanId':p,'ModuleId':m,'Enabled':'false'})
        check(scalar('SELECT Enabled FROM PlanModules WHERE PlanId=? AND ModuleId=?',(p,m))==0,'Join row not updated')
        client.post('/Configuration/Delete',{'planId':p,'moduleId':m},'/Configuration')
        check(scalar('SELECT count(*) FROM PlanModules WHERE PlanId=? AND ModuleId=?',(p,m))==0,'Join row not removed')
    test('Many-to-many module configuration','Add, disable and remove module assignment','Composite relationship persists all three operations',config)
    def invoice_validation():
        before=scalar('SELECT count(*) FROM Invoices')
        data={'SubscriptionId':ids['Subscriptions'],'PeriodStart':'2026-11-15','IssuedOn':'2026-11-20','DueOn':'2026-11-01'}
        s,h,u=client.post('/Invoices/Create',data)
        check('Only active' in h and 'first day' in h and 'on or after' in h,'Missing invoice rules')
        check(scalar('SELECT count(*) FROM Invoices')==before,'Invalid invoice persisted')
    test('Invoice business validation','Paused subscription, mid-month and due-before-issue','All invalid fields rejected',invoice_validation)
    def invoice_create():
        table,data,key,val=specs['Subscriptions'];data=dict(data,Id=ids['Subscriptions'],Status='Active')
        client.post('/Subscriptions/Edit/'+str(ids['Subscriptions']),data)
        payload={'SubscriptionId':ids['Subscriptions'],'PeriodStart':'2026-11-01','IssuedOn':'2026-11-01','DueOn':'2026-11-15','Amount':'0.01','Reference':'INJECTED'}
        s,h,u=client.post('/Invoices/Create',payload)
        check('/Details/' in u,'Invoice not created')
        row=sql('SELECT Id,Subtotal,DiscountAmount,Amount,Reference FROM Invoices WHERE SubscriptionId=? AND PeriodStart LIKE ?',(ids['Subscriptions'],'2026-11-01%'))[0]
        ids['Invoices']=row[0];check(float(row[1])==120 and float(row[2])==18 and float(row[3])==102 and row[4]!='INJECTED',str(row)); return str(row)
    test('Invoice calculated by server','Invoice $120 plan with 15% discount; inject amount/reference','Subtotal120, discount18, amount102; generated reference',invoice_create)
    test('Duplicate billing month blocked','Generate same subscription/month again','Custom duplicate error; one invoice',lambda:check('already exists' in client.post('/Invoices/Create',{'SubscriptionId':ids['Subscriptions'],'PeriodStart':'2026-11-01','IssuedOn':'2026-11-01','DueOn':'2026-11-15'})[1],'Duplicate not rejected'))
    def payment():
        i=ids['Invoices']; client.post('/Invoices/MarkPaid/'+str(i),{},'/Invoices/Details/'+str(i))
        check(scalar('SELECT Status FROM Invoices WHERE Id=?',(i,))=='Paid','Payment not stored')
        h=client.post('/Invoices/MarkPaid/'+str(i),{},'/Invoices/Details/'+str(i))[1]
        check('already paid' in h,'Repeated payment not explained')
    test('Invoice payment recording','Mark invoice paid then repeat','Status Paid; repeated payment rejected',payment)
    def bounds():
        before=scalar('SELECT count(*) FROM Discounts')
        base={'Name':'Boundary Discount','Description':'Boundary discount','StartsOn':'2026-01-01','EndsOn':'2027-01-01','Enabled':'true'}
        for value in ['-1','101','not-a-number']:
            response=client.post('/Discounts/Create',dict(base,Percentage=value))
            check('/Create' in response[2] and scalar('SELECT count(*) FROM Discounts')==before,'Invalid percentage accepted: '+value)
        for value in ['0','100']:
            response=client.post('/Discounts/Create',dict(base,Name='Boundary '+value,Percentage=value))
            check('/Details/' in response[2], 'Boundary rejected: '+value)
        return 'Rejected -1,101,non-number; accepted 0 and 100'
    test('Discount boundaries and type errors','Submit -1,101,text,0,100 percentages','Only inclusive 0..100 numeric values accepted',bounds)
    def foreign_keys():
        before=scalar('SELECT count(*) FROM Subscriptions')
        payload={'CustomerId':'999999','PlanId':'999999','StartDate':'2026-10-01','Status':'Forged','DiscountId':'999999'}
        h=client.post('/Subscriptions/Create',payload)[1]
        check('existing customer' in h and 'existing plan' in h and 'enabled discount' in h and scalar('SELECT count(*) FROM Subscriptions')==before,'Forged associations not rejected')
    test('Forged relationship IDs and status','Submit nonexistent customer,plan,discount plus forged status','Custom errors; no subscription inserted',foreign_keys)
    def maximum_date():
        payload={'SubscriptionId':ids['Subscriptions'],'PeriodStart':'9999-12-01','IssuedOn':'9999-12-01','DueOn':'9999-12-15'}
        status,html,url=client.post('/Invoices/Create',payload)
        check(status==200 and '/Details/' in url,'Maximum billing month failed: HTTP '+str(status))
        check(scalar("SELECT count(*) FROM Invoices WHERE SubscriptionId=? AND PeriodStart LIKE '9999-12-01%'",(ids['Subscriptions'],))==1,'Maximum month not stored')
        return 'December 9999 invoice created without DateTime overflow'
    test('Maximum supported billing date','Generate invoice for 9999-12-01','Valid maximum month saved without server error',maximum_date)
    def midpoint_rounding():
        plan=dict(specs['Plans'][1],Id=ids['Plans'],MonthlyPrice='0.05')
        discount=dict(specs['Discounts'][1],Id=ids['Discounts'],Percentage='10')
        client.post('/Plans/Edit/'+str(ids['Plans']),plan)
        client.post('/Discounts/Edit/'+str(ids['Discounts']),discount)
        payload={'SubscriptionId':ids['Subscriptions'],'PeriodStart':'2026-12-01','IssuedOn':'2026-12-01','DueOn':'2026-12-15'}
        status,html,url=client.post('/Invoices/Create',payload)
        check(status==200 and '/Details/' in url,'Midpoint invoice not created')
        row=sql("SELECT Subtotal,DiscountAmount,Amount FROM Invoices WHERE SubscriptionId=? AND PeriodStart LIKE '2026-12-01%'",(ids['Subscriptions'],))[0]
        check([float(x) for x in row]==[.05,.01,.04],str(row))
        return 'Subtotal $0.05, 10% discount rounded from $0.005 to $0.01, final $0.04'
    test('Half-cent invoice rounding','Set plan $0.05 and discount 10%; invoice December 2026','Midpoint rounds away from zero to discount $0.01 and total $0.04',midpoint_rounding)
    test('Audit immutability via routes','Request audit create/edit/delete endpoints','No writable audit routes',lambda:check(all(client.get('/Audit/'+a)[0] in (404,405) for a in ['Create','Edit/1','Delete/1']),'Writable audit route found'))
    test('Missing record handling','GET details for missing customer','HTTP 404',lambda:check(client.get('/Customers/Details/999999')[0]==404,'Missing record not 404'))
    def admin_crud():
        data={'FullName':'Integration Admin','Email':'testadmin@example.com','Phone':'+61 2 5550 9999','Department':'Testing','password':'TestAdmin!2026'}
        s,h,u=client.post('/Administrators/Create',data);check('/Details/' in u,'Admin creation failed')
        aid=scalar("SELECT Id FROM People WHERE Email='testadmin@example.com'");data.update(Id=aid,Email='editedadmin@example.com')
        client.post('/Administrators/Edit/'+str(aid),data)
        check(scalar('SELECT Email FROM AspNetUsers WHERE AdministratorId=?',(aid,))=='editedadmin@example.com','Identity email not synchronized')
        client.post('/Administrators/Delete/'+str(aid),{'id':aid})
        check(scalar('SELECT count(*) FROM AspNetUsers WHERE AdministratorId=?',(aid,))==0 and scalar('SELECT count(*) FROM People WHERE Id=?',(aid,))==0,'Admin not removed')
    test('Administrator account CRUD','Create credentials, update email, delete unused account','Identity and domain profile remain synchronized',admin_crud)
    test('Own administrator protected','POST delete seeded Alex','Deletion rejected and account retained',lambda:check('cannot delete your own' in client.post('/Administrators/Delete/101',{'id':101})[1],'Own deletion not blocked'))
    def password_validation():
        before=scalar('SELECT count(*) FROM People');data={'FullName':'Weak Admin','Email':'weak@example.com','Phone':'+61 2 5550 9998','Department':'Test','password':'weak'}
        h=client.post('/Administrators/Create',data)[1]
        check('at least 12' in h and scalar('SELECT count(*) FROM People')==before,'Password/profile rollback validation failed')
    test('Weak-password atomic rollback','Create admin using weak password','Custom password errors and no orphan profile',password_validation)
    # Delete a separate unused subscription because an invoiced subscription is intentionally immutable history.
    def deletion(route):
        ident=ids[route];table=specs[route][0];client.post('/'+route+'/Delete/'+str(ident),{'id':ident})
        check(scalar('SELECT count(*) FROM '+table+' WHERE Id=?',(ident,))==0,'Delete did not persist')
    test('Usage delete','Delete temporary usage','Usage row removed',lambda:deletion('Usage'))
    # The invoiced fixture retains its related customer, plan and discount. Fresh fixtures exercise normal deletion.
    for route in ['Customers','Plans','Modules','Discounts','Subscriptions']:
        def fresh_delete(r=route):
            table,data,key,value=specs[r];data=dict(data)
            if r=='Customers':data['Email']='delete@example.com';key='Email';value=data[key]
            elif r in ['Plans','Modules','Discounts']:data['Name']='Delete '+r;key='Name';value=data[key]
            else:data['CustomerId']=1;key='Id';value=None
            s,h,u=client.post('/'+r+'/Create',data);check('/Details/' in u,'Fresh delete fixture not created')
            ident=int(u.rstrip('/').split('/')[-1]);client.post('/'+r+'/Delete/'+str(ident),{'id':ident})
            check(scalar('SELECT count(*) FROM '+table+' WHERE Id=?',(ident,))==0,'Delete failed')
        test(route+' delete','Create unused fixture then delete','Record removed without orphan references',fresh_delete)
    def nonzero_audit_targets():
        rows=sql("SELECT Action,Target FROM AuditEntries WHERE Action IN ('Created Subscription','Created UsageRecord')")
        check(len(rows)>=2,'Missing creation audit entries')
        for action,target in rows:
            check(target and not target.endswith(': 0') and not target.endswith('#0') and not target.endswith('# 0'),action+': '+target)
        return str(rows)
    test('Creation audit targets identify persisted relationships','Inspect new subscription and usage creation audit targets','Meaningful identifiers; no unsaved ID zero',nonzero_audit_targets)
    def history_customer_reassignment():
        sid=1
        row=sql('SELECT CustomerId,PlanId,DiscountId,StartDate,Status FROM Subscriptions WHERE Id=?',(sid,))[0]
        other=scalar("SELECT Id FROM People WHERE PersonType='Customer' AND Id!=? LIMIT 1",(row[0],))
        payload=dict(Id=sid,CustomerId=other,PlanId=row[1],DiscountId=row[2] or '',StartDate=row[3][:10],Status=row[4])
        status,html,url=client.post('/Subscriptions/Edit/1',payload)
        check(status==200 and '/Edit/' in url,'Historical customer reassignment not rejected')
        check(scalar('SELECT CustomerId FROM Subscriptions WHERE Id=1')==row[0],'Historical invoice customer changed')
        return 'Existing invoice/usage history retains original customer'
    test('Historical subscription customer protected','Change customer on seeded subscription with invoice and usage','Reject edit and preserve original customer association',history_customer_reassignment)
    def own_email_edit_and_write():
        row=sql('SELECT FullName,Email,Phone,Department FROM People WHERE Id=101')[0]
        payload=dict(Id=101,FullName=row[0],Email='alex.changed@example.com',Phone=row[2],Department=row[3])
        status,html,url=client.post('/Administrators/Edit/101',payload)
        check(status==200 and '/Details/' in url,'Own email edit failed')
        module={'Name':'After Own Email Change','Description':'Verifies actor lookup uses immutable identity'}
        try:
            status,html,url=client.post('/Modules/Create',module)
            check(status==200 and '/Details/' in url,'Write failed after own email changed')
            mid=scalar("SELECT Id FROM Modules WHERE Name='After Own Email Change'")
            actor=scalar("SELECT AdministratorId FROM AuditEntries WHERE Action='Created Module' AND Target LIKE '%After Own Email Change%' ORDER BY Id DESC LIMIT 1")
            check(actor==101,'Write audit attributed to wrong actor')
            client.post('/Modules/Delete/'+str(mid),{'id':mid})
        finally:
            payload['Email']=row[1]
            response=client.post('/Administrators/Edit/101',payload)
            check(response[0]==200 and scalar('SELECT Email FROM People WHERE Id=101')==row[1],'Original own email not restored')
        return 'Own email changed; subsequent create audited to administrator101; email restored'
    test('Own email change preserves write authorization and audit','Edit signed-in admin email, create module, restore email','Existing session can write and audit references immutable account identity',own_email_edit_and_write)
    def audit():
        count=scalar('SELECT count(*) FROM AuditEntries');check(count>25,'Audit events missing')
        texts=' '.join(' '.join(map(str,row)) for row in sql('SELECT Action,Target FROM AuditEntries'))
        check('QuTechLab3!' not in texts and 'TestAdmin!' not in texts,'Password leaked to audit')
        return str(count)+' audit rows; no submitted passwords in Action/Target'
    test('Audit write coverage and privacy','Inspect audit rows after CRUD/auth actions','Actions recorded without passwords',audit)
    def logout():
        client.post('/Account/Logout',{},'/');check('/Account/Login' in client.get('/Customers')[2],'Logout did not revoke cookie')
    test('Logout revokes access','POST logout then request customers','Returns to sign-in',logout)
    def lockout():
        other=Client()
        email=scalar("SELECT Email FROM AspNetUsers WHERE AdministratorId=102")
        for _ in range(5): other.post('/Account/Login',{'Email':email,'Password':'WrongPassword!2026'})
        value=scalar('SELECT LockoutEnd FROM AspNetUsers WHERE AdministratorId=102')
        check(value is not None,'Failed attempts did not set lockout')
        status,html,url=other.post('/Account/Login',{'Email':email,'Password':'QuTechLab3!2026'})
        check('/Account/Login' in url and 'Unable to sign in.' in html,'Locked account signed in')
        return 'Five failures lock the separate seeded account; correct password cannot bypass lockout'
    test('Repeated credential failures lock account','Five wrong passwords then correct one on separate seeded account','Temporary lockout and generic response',lockout)


def invalid_empty(client,route,table):
    before=scalar('SELECT count(*) FROM '+table);s,h,u=client.post('/'+route+'/Create',{})
    check(s==200 and 'validation' in h and '/Create' in u,'Invalid form not redisplayed')
    check(scalar('SELECT count(*) FROM '+table)==before,'Invalid row persisted')

if __name__=='__main__':
    parser=argparse.ArgumentParser()
    parser.add_argument('--no-build',action='store_true')
    parser.add_argument('--output-dir',type=Path,default=OUT,help='Disposable test database and evidence directory')
    args=parser.parse_args()
    # Fail before touching a database if the fixed test port belongs to another service.
    with socket.socket(socket.AF_INET,socket.SOCK_STREAM) as probe:
        probe.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
        try: probe.bind(('127.0.0.1',5086))
        except OSError: raise SystemExit('Port 5086 is already in use. Stop that service or use another test environment.')
    OUT=args.output_dir.resolve()
    OUT.mkdir(parents=True,exist_ok=True)
    DB=OUT/'lab3-test.db'
    try:
        if not args.no_build: subprocess.run(['dotnet','build',str(APP/'JustinLee-Lab2.csproj'),'--nologo'],check=True)
        for suffix in ['', '-wal','-shm']:
            path=Path(str(DB)+suffix)
            if path.exists():path.unlink()
        start();run()
    finally:
        stop()
        (OUT/'test-results.log').write_text('\n'.join(f"{x['id']} {x['status']} {x['title']}: {x['actual']}" for x in RESULTS)+'\n')
    passed=sum(x['status']=='PASS' for x in RESULTS);print(f'{passed}/{len(RESULTS)} test cases passed')
    raise SystemExit(0 if RESULTS and passed==len(RESULTS) else 1)
