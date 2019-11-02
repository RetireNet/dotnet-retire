#!/bin/bash


echo "### AssertCMD - Executing command '$2'"
eval $2
code=$?

if [ $code -ne $1 ]; then
  echo "### AssertCMD - FAIL: Expected exit code $1 is not equal to $code" 1>&2
  exit 1
fi

echo "### AssertCMD - PASSED"
exit 0