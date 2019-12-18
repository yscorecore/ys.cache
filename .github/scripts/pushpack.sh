#/bin/bash

 for file in ` ls $1/*.nupkg `  
    do  
        dotnet nuget push $file -k $2 -s  https://api.nuget.org/v3/index.json --skip-duplicate
        
    done  
echo 'push package success.'