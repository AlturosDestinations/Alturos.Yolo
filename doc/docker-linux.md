Checkout and build darknet

docker pull gcc
docker run -it gcc /bin/bash
apt-get update
apt-get install cmake -y

git clone https://github.com/AlexeyAB/darknet.git
cd darknet
sed -i 's/LIBSO=0/LIBSO=1/g' Makefile

mkdir build-release
cd build-release
cmake ..
make
make install



Copy files from an other console window to local disk
docker ps
docker cp 6c1e7b0ee215:/darknet/build_release C:\temp




Required on the dotnet docker container
apt-get update
apt-get install libgomp1