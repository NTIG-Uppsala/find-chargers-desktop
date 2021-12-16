del build\*

dotnet publish desktop-app\ -r win10-x64 -p:PublishSingleFile=true --self-contained -c Release
dotnet publish desktop-app\ -r linux-x64 -p:PublishSingleFile=true --self-contained -c Release
dotnet publish desktop-app\ -r osx-x64 -p:PublishSingleFile=true --self-contained -c Release

mkdir build
copy find-chargers-desktop\desktop-app\bin\Release\net6.0\osx-x64\publish\find-chargers-desktop.exe build/find-chargers-windows.exe
copy find-chargers-desktop\desktop-app\bin\Release\net6.0\osx-x64\publish\find-chargers-desktop build/find-chargers-linux
copy find-chargers-desktop\desktop-app\bin\Release\net6.0\osx-x64\publish\find-chargers-desktop build/find-chargers-osx

echo "Done Files are in build\"