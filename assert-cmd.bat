@echo off
CD /D %~dp0

echo "### AssertCMD - Executing command '%2 %3'"
%2 %3
SET code=%ERRORLEVEL%

if %code% neq %1 (
  echo "### AssertCMD - FAIL: Expected exit code %1 is not equal to %code%" 1>&2
  exit 1
)

echo "### AssertCMD - PASSED"
exit 0