"""Verify the unchanged Lab 2 starting point, not Lab 3 implementation."""
from pathlib import Path
import os, socket, subprocess, sys, tempfile, time
from urllib.request import urlopen
root = Path(__file__).resolve().parents[1]
port = 5084
with socket.socket() as probe:
    try:
        probe.bind(("127.0.0.1", port))
    except OSError:
        sys.exit("Port 5084 is occupied. Stop that service or choose a free port in this script.")
subprocess.run(["dotnet", "build", "JustinLee-Lab2.sln", "--nologo"], cwd=root, check=True)
env = dict(os.environ, DOTNET_USE_POLLING_FILE_WATCHER="1", DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE="false")
with tempfile.TemporaryFile(mode="w+") as log:
    app = subprocess.Popen(["dotnet", "bin/Debug/net10.0/JustinLee-Lab2.dll", "--urls", f"http://127.0.0.1:{port}"], cwd=root / "JustinLee-Lab2", env=env, stdout=log, stderr=subprocess.STDOUT)
    try:
        for attempt in range(60):
            if app.poll() is not None:
                raise RuntimeError("Application stopped before becoming ready")
            try:
                with urlopen(f"http://127.0.0.1:{port}", timeout=1) as response:
                    if response.status == 200:
                        break
            except OSError:
                time.sleep(0.5)
        else:
            raise RuntimeError("Application did not become ready")
        subprocess.run([sys.executable, "scripts/smoke_test.py", f"http://127.0.0.1:{port}"], cwd=root, check=True)
    except Exception:
        log.seek(0)
        print(log.read())
        raise
    finally:
        if app.poll() is None:
            app.terminate()
            try:
                app.wait(timeout=10)
            except subprocess.TimeoutExpired:
                app.kill()
                app.wait()
