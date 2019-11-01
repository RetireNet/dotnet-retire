@echo off
CD /D %~dp0

echo "### AssertCMD - Executing command '%2 %3'"
%2 %3

if %ERRORLEVEL% neq %1 (
  echo "### AssertCMD - FAIL: Expected exit code %1 is not equal to %ERRORLEVEL%" 1>&2
  exit 999
)

echo "### AssertCMD - PASSED"
exit 0