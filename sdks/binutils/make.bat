@echo off
setlocal
set PATH=C:\MinGW64\bin;%PATH%
g++ binutils.cpp decodedline.cpp -Wall -static-libgcc -shared -m32 -lbfd -liberty -lole32 -std=c++0x -o binutils.dll -Wl,--kill-at -s