#!/bin/bash
dotnet test ${GST_APPLICATION_NAME}.sln --filter TestCategory=${TESTCATEGORY} --logger "trx;logfilename=testresult.trx" 2>&1 > /src/${GST_APPLICATION_NAME}_testexecution.log || echo "There were failing tests!"
dotnet specsync publish-test-results --testResultFile *UITests/TestResults/testresult.trx --testConfiguration "Windows 10"

