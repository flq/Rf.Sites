rmdir publish -fo -r -ErrorAction SilentlyContinue
msbuild build.xml /p:Local=True