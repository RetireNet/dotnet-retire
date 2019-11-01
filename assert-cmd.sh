#!/bin/bash


echo "### AssertCMD - Executing command '$2'"
eval $2

if [ $? -ne $1 ]; then
  echo "### AssertCMD - FAIL: Expected exit code $1 is not equal to $?" 1>&2
  exit 999
fi

echo "### AssertCMD - PASSED"
exit 0