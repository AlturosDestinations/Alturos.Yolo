Checkout and build darknet
```
docker pull gcc
docker run -it gcc /bin/bash
apt-get update && apt-get install build-essential cmake git libgtk2.0-dev pkg-config libavcodec-dev libavformat-dev libswscale-dev libopencv-dev -y

git clone https://github.com/AlexeyAB/darknet.git
cd darknet
sed -i 's/LIBSO=0/LIBSO=1/g' Makefile
sed -i 's/OPENCV=0/OPENCV=1/g' Makefile

make
```


Copy files from an other console window to local disk
```
docker ps
docker cp 6c1e7b0ee215:/darknet/build_release C:\temp
```


Required on the dotnet docker container
```
apt-get update
apt-get install libgomp1
```
