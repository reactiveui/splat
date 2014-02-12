MDTOOL ?= /Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool

.PHONY: all clean

all: Splat.dll

Splat.dll:
	$(MDTOOL) build -c:Release Splat-XamarinStudio.sln

clean:
	$(MDTOOL) build -t:Clean -c:Release Splat-XamarinStudio.sln
