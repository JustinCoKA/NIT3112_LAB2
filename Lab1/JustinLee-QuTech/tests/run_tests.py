"""Black-box console tests. Run with Python 3 from any working directory."""
from pathlib import Path
import subprocess, json
root=Path(__file__).resolve().parents[1]
project=root/'JustinLee-QuTech/JustinLee-QuTech.csproj'
subprocess.run(['dotnet','build',str(project)],check=True)
dll=root/'JustinLee-QuTech/bin/Debug/net10.0/JustinLee-QuTech.dll'
results=[]
def run(name,inputs,expected,forbidden=()):
 p=subprocess.run(['dotnet',str(dll)],input=inputs,text=True,capture_output=True,timeout=10)
 ok=p.returncode==0 and all(x in p.stdout for x in expected) and all(x not in p.stdout for x in forbidden)
 results.append(dict(name=name,input=inputs,expected=expected,status='PASS' if ok else 'FAIL',output=p.stdout))
 return p.stdout
run('Welcome and four plans','A\nD\n',['Justin Lee (Administrator)','Startup','Standard','Premium','Enterprise','$10.00','$20.00','$30.00','$40.00','Goodbye'])
run('Empty summary','C\nD\n',['No subscriptions yet.'])
entries=[('Woolies',1,20,'No','$400.00'),('Coles',2,25,'JUST','$637.50'),('Harbour Studio',0,1,'No','$10.00'),('Summit Logistics',3,100,'JUST','$3400.00'),('Greenfield Dental',1,10,'just','$170.00')]
inputs=''.join(f'B\n{n}\n{p}\n{u}\n{c}\n' for n,p,u,c,t in entries)+'C\nD\n'
out=run('Five subscriptions and summary',inputs,[*(f'Saved subscription for {n}' for n,_,_,_,_ in entries),*(f'Total monthly price: {t}' for *_,t in entries),'Subscriptions: 5 | Combined monthly total: $4617.50'])
# Each row must independently contain all five mandatory summary fields.
rows=out[out.index('All Subscriptions'):].splitlines()
for n,p,u,c,t in entries:
 matching=[r for r in rows if r.startswith(n+' ')]
 assert len(matching)==1 and t in matching[0] and matching[0].endswith('N' if c=='No' else 'Y')
 fields=[f.strip() for f in matching[0].split('|')]
 assert fields == [n,['Startup','Standard','Premium','Enterprise'][p],str(u),t,'N' if c=='No' else 'Y']
run('Invalid inputs and recovery','?\nB\n\nCheck Client\n4\n-1\ntext\n1\n0\n101\n1.5\nabc\n999999999999999999\n20\nBAD\nNo\nC\nD\n',['Invalid option.','Business name must','Enter a whole number from 0 to 3.','Enter a whole number from 1 to 100.','Invalid discount code.','Subscriptions: 1','Total monthly price: $400.00'])
for plan,price in enumerate([10,20,30,40]):
 run(f'Plan {plan} upper boundary no discount',f'B\nBoundary\n{plan}\n100\nNo\nD\n',[f'Total monthly price: ${price*100:.2f}'])
run('Trimmed lowercase selections',' b \n Trim Client \n0\n1\n just \nc\nd\n',['Saved subscription for Trim Client','Total monthly price: $8.50','Discount: Y'])
run('Name length limit','B\n'+'A'*61+'\nValid\n0\n1\nNo\nD\n',['Business name must','Saved subscription for Valid'])
run('EOF during incomplete entry','B\nUnfinished\n0\n',['Input ended. Any incomplete entry was not saved.'],['Saved subscription'])
run('Fresh process clears session','C\nD\n',['No subscriptions yet.'],['Woolies'])
(root/'tests/results.json').write_text(json.dumps(results,indent=2))
print('\n'.join(f"{r['status']}: {r['name']}" for r in results))
assert all(r['status']=='PASS' for r in results)
