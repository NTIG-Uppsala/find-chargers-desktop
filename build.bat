del build\*

dotnet publish desktop-app\ -r win10-x64 -p:PublishSingleFile=true --self-contained -c Release
dotnet publish desktop-app\ -r linux-x64 -p:PublishSingleFile=true --self-contained -c Release
dotnet publish desktop-app\ -r osx-x64 -p:PublishSingleFile=true --self-contained -c Release

mkdir build
copy desktop-app\bin\Release\net6.0\win10-x64\publish\find-chargers-desktop.exe build\find-chargers-windows.exe
copy desktop-app\bin\Release\net6.0\linux-x64\publish\find-chargers-desktop build\find-chargers-linux
copy desktop-app\bin\Release\net6.0\osx-x64\publish\find-chargers-desktop build\find-chargers-osx

echo "Done Files are in build\"