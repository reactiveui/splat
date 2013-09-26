using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Splat
{
    class FallbackBitmapLoader : IBitmapLoader
    {
        public async Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            ImagingFactory factory = new ImagingFactory();
            var decoder = factory.CreateDecoderFromStream(sourceStream, WICDecodeOptions.WICDecodeMetadataCacheOnLoad);
            var frame = decoder.GetFrame(0);
            var frameSize = frame.GetSize();

            BitmapSource convertedSource;
            if (!frame.GetPixelFormat().Equals(NativeMethods.WICPixelFormat32bppBGRA))
            {
                FormatConverter converter = factory.CreateFormatConverter();
                converter.Initialize(frame, NativeMethods.WICPixelFormat32bppBGRA);
                convertedSource = converter;
            }
            else
                convertedSource = frame;

            byte[] buffer = convertedSource.CopyPixels();

            WriteableBitmap bmp = new WriteableBitmap(frameSize.Width, frameSize.Height);
            using (var stream = bmp.PixelBuffer.AsStream())
                stream.Write(buffer, 0, buffer.Length);
            bmp.Invalidate();

            return new WriteableBitmapImageBitmap(bmp);
        }

        public async Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            throw new NotImplementedException();
        }

        public IBitmap Create(float width, float height)
        {
            throw new NotImplementedException();
        }

        class ImagingFactory
        {

            IWICImagingFactory comObject;
            IntPtr nativePointer;
            IntPtr riidPointer;

            public ImagingFactory()
            {
                Guid riid = typeof(IWICImagingFactory).GetTypeInfo().GUID;
                byte[] riidBytes = riid.ToByteArray();
                riidPointer = Marshal.AllocHGlobal(riidBytes.Length);
                Marshal.Copy(riidBytes, 0, riidPointer, riidBytes.Length);
                MultiQueryInterface localQuery = new MultiQueryInterface()
                {
                    InterfaceIID = riidPointer,
                    IUnknownPointer = IntPtr.Zero,
                    ResultCode = 0,
                };
                var result = NativeMethods.CoCreateInstanceFromApp(NativeMethods.CLSID_WICImagingFactory, IntPtr.Zero, CLSCTX.CLSCTX_INPROC_SERVER, IntPtr.Zero, 1, ref localQuery);
                if (result != NativeMethods.S_OK || localQuery.ResultCode != NativeMethods.S_OK)
                    throw new Exception("CoCreateInstanceFromApp failed");
                this.nativePointer = localQuery.IUnknownPointer;
                this.comObject = (IWICImagingFactory)Marshal.GetObjectForIUnknown(nativePointer);
            }

            ~ImagingFactory()
            {
                Marshal.FreeHGlobal(riidPointer);
                // todo: do other classes as well as this one need the com objects to be freed? possible memory leak
            }

            public BitmapDecoder CreateDecoderFromStream(Stream stream, WICDecodeOptions wicDecodeOptions)
            {
                IntPtr nativePointer;
                Guid nullGuid = Guid.Empty;
                var result = comObject.CreateDecoderFromStream(new ManagedIStream(stream), ref nullGuid, wicDecodeOptions, out nativePointer);
                if (result != NativeMethods.S_OK)
                    throw new Exception("CreateDecoderFromStream failed");
                return new BitmapDecoder(nativePointer);
            }

            public FormatConverter CreateFormatConverter()
            {
                IntPtr nativePointer;
                var result = comObject.CreateFormatConverter(out nativePointer);
                if (result != NativeMethods.S_OK)
                    throw new Exception("CreateFormatConverter failed");
                return new FormatConverter(nativePointer);
            }
        }

        class BitmapDecoder
        {
            IWICBitmapDecoder comObject;
            IntPtr nativePointer;

            internal BitmapDecoder(IntPtr nativePointer)
            {
                this.nativePointer = nativePointer;
                this.comObject = (IWICBitmapDecoder)Marshal.GetObjectForIUnknown(nativePointer);
            }

            public BitmapFrameDecode GetFrame(uint index)
            {
                IntPtr nativePointer;
                var result = comObject.GetFrame(index, out nativePointer);
                if (result != NativeMethods.S_OK)
                    throw new Exception("GetFrame failed");
                return new BitmapFrameDecode(nativePointer);
            }
        }

        class BitmapSource
        {
            IWICBitmapSource comObject;
            public IntPtr NativePointer { get; private set; }

            internal BitmapSource(IntPtr nativePointer)
            {
                NativePointer = nativePointer;
                comObject = (IWICBitmapSource)Marshal.GetObjectForIUnknown(NativePointer);
            }

            public System.Drawing.Size GetSize()
            {
                uint width, height;
                comObject.GetSize(out width, out height);
                // this will throw an exception if dimensions are > Int32.MaxValue
                // in fact all of this code assumes dimensions will not overflow under arithmetic
                return new System.Drawing.Size(checked((int)width), checked((int)height));
            }

            public Guid GetPixelFormat()
            {
                Guid pixelFormat;
                comObject.GetPixelFormat(out pixelFormat);
                return pixelFormat;
            }

            public byte[] CopyPixels()
            {
                uint width, height;
                GetSizeInternal(out width, out height);
                byte[] pixels = new byte[width * height * 4];
                uint bufferSize = width * height * 32;
                IntPtr buffer = Marshal.AllocCoTaskMem(checked((int)bufferSize));
                comObject.CopyPixels(
                    IntPtr.Zero,
                    width * 4,
                    bufferSize,
                    buffer);
                Marshal.Copy(buffer, pixels, 0, pixels.Length);
                Marshal.FreeCoTaskMem(buffer);
                return pixels;
            }

            private void GetSizeInternal(out uint width, out uint height)
            {
                comObject.GetSize(out width, out height);
            }
        }

        class BitmapFrameDecode : BitmapSource
        {
            internal BitmapFrameDecode(IntPtr nativePointer) : base(nativePointer) { }
        }

        class FormatConverter : BitmapSource
        {
            IWICFormatConverter comObject;

            internal FormatConverter(IntPtr nativePointer)
                : base(nativePointer)
            {
                this.comObject = (IWICFormatConverter)Marshal.GetObjectForIUnknown(NativePointer);
            }

            public void Initialize(BitmapSource bitmapSource, Guid pixelFormat)
            {
                comObject.Initialize(bitmapSource.NativePointer, pixelFormat, 0, IntPtr.Zero, 0.0f, 0);
            }
        }

        class ManagedIStream : IStream
        {
            Stream _stream;

            public ManagedIStream(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException("stream");
                _stream = stream;
            }

            public void Read(byte[] pv, int cb, IntPtr pcbRead)
            {
                int val = _stream.Read(pv, 0, cb);
                if (pcbRead != IntPtr.Zero)
                    Marshal.WriteInt32(pcbRead, val);
            }

            public void Write(byte[] pv, int cb, IntPtr pcbWritten)
            {
                _stream.Write(pv, 0, cb);
                if (pcbWritten != IntPtr.Zero)
                    Marshal.WriteInt32(pcbWritten, cb);
            }

            public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
            {
                SeekOrigin origin;
                switch (dwOrigin)
                {
                    case 0: origin = SeekOrigin.Begin; break;
                    case 1: origin = SeekOrigin.Current; break;
                    case 2: origin = SeekOrigin.End; break;
                    default: throw new ArgumentOutOfRangeException("dwOrigin");
                }

                long val = _stream.Seek(dlibMove, origin);
                if (plibNewPosition != IntPtr.Zero)
                    Marshal.WriteInt64(plibNewPosition, val);
            }

            public void SetSize(long libNewSize)
            {
                throw new NotSupportedException();
            }

            public void Stat(out STATSTG pstatstg, int grfStatFlag)
            {
                pstatstg = new STATSTG
                {
                    type = 2,
                    cbSize = _stream.Length,
                };

                if (_stream.CanRead && _stream.CanWrite)
                    pstatstg.grfMode = 0x00000002;
                else if (_stream.CanWrite)
                    pstatstg.grfMode = 0x00000001;
                else if (_stream.CanRead)
                    pstatstg.grfMode = 0x00000000;
                else
                    throw new IOException();
            }

            public void Clone(out IStream ppstm)
            {
                throw new NotSupportedException();
            }

            public void Commit(int grfCommitFlags)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
            {
                throw new NotImplementedException();
            }

            public void LockRegion(long libOffset, long cb, int dwLockType)
            {
                throw new NotImplementedException();
            }

            public void Revert()
            {
                throw new NotImplementedException();
            }

            public void UnlockRegion(long libOffset, long cb, int dwLockType)
            {
                throw new NotImplementedException();
            }
        }
    }
}
