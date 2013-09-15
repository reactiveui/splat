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
            if (!frame.GetPixelFormat().Equals(WICPixelFormat32bppBGRA))
            {
                FormatConverter converter = factory.CreateFormatConverter();
                converter.Initialize(frame, WICPixelFormat32bppBGRA);
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


        // understatement of the year: here be dragons


        [Flags]
        enum CLSCTX : uint
        {
            CLSCTX_INPROC_SERVER = 0x1,
            CLSCTX_INPROC_HANDLER = 0x2,
            CLSCTX_LOCAL_SERVER = 0x4,
            CLSCTX_INPROC_SERVER16 = 0x8,
            CLSCTX_REMOTE_SERVER = 0x10,
            CLSCTX_INPROC_HANDLER16 = 0x20,
            CLSCTX_RESERVED1 = 0x40,
            CLSCTX_RESERVED2 = 0x80,
            CLSCTX_RESERVED3 = 0x100,
            CLSCTX_RESERVED4 = 0x200,
            CLSCTX_NO_CODE_DOWNLOAD = 0x400,
            CLSCTX_RESERVED5 = 0x800,
            CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
            CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
            CLSCTX_NO_FAILURE_LOG = 0x4000,
            CLSCTX_DISABLE_AAA = 0x8000,
            CLSCTX_ENABLE_AAA = 0x10000,
            CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,
            CLSCTX_ACTIVATE_32_BIT_SERVER = 0x40000,
            CLSCTX_ACTIVATE_64_BIT_SERVER = 0x80000,
            CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,
            CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
            CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MultiQueryInterface
        {
            public IntPtr InterfaceIID;
            public IntPtr IUnknownPointer;
            public int ResultCode;
        }

        enum WICDecodeOptions : uint
        {
            WICDecodeMetadataCacheOnDemand = 0,
            WICDecodeMetadataCacheOnLoad = 1,
            WICMETADATACACHEOPTION_FORCE_DWORD = 0x7fffffff
        }

        const int S_OK = 0;

        static readonly Guid WICPixelFormat1bppIndexed = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc901");
        static readonly Guid WICPixelFormatBlackWhite = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc905");
        static readonly Guid WICPixelFormat8bppIndexed = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc904");
        static readonly Guid WICPixelFormatDontCare = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc900");
        static readonly Guid WICPixelFormat24bppBGR = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc90c");
        static readonly Guid WICPixelFormat32bppBGRA = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc90e");
        static readonly Guid WICPixelFormat48bppRGB = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc915");

        class ImagingFactory
        {
            static readonly Guid CLSID_WICImagingFactory = new Guid("cacaf262-9370-4615-a13b-9f5539da4c0a");

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
                var result = CoCreateInstanceFromApp(CLSID_WICImagingFactory, IntPtr.Zero, CLSCTX.CLSCTX_INPROC_SERVER, IntPtr.Zero, 1, ref localQuery);
                if (result != S_OK || localQuery.ResultCode != S_OK)
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
                if (result != S_OK)
                    throw new Exception("CreateDecoderFromStream failed");
                return new BitmapDecoder(nativePointer);
            }

            public FormatConverter CreateFormatConverter()
            {
                IntPtr nativePointer;
                var result = comObject.CreateFormatConverter(out nativePointer);
                if (result != S_OK)
                    throw new Exception("CreateFormatConverter failed");
                return new FormatConverter(nativePointer);
            }

            [DllImport("ole32.dll", ExactSpelling = true, EntryPoint = "CoCreateInstanceFromApp", PreserveSig = true)]
            private static extern int CoCreateInstanceFromApp(
                [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
                IntPtr pUnkOuter,
                CLSCTX dwClsContext,
                IntPtr reserved,
                int countMultiQuery,
                ref MultiQueryInterface query);

            [Guid("ec5ec8a9-c395-4314-9c77-54d7a935ff70")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            [ComImport]
            interface IWICImagingFactory
            {
                void CreateDecoderFromFilenameDummy();
                int CreateDecoderFromStream(
                    IStream pIStream,
                    ref Guid pguidVendor,
                    WICDecodeOptions metadataOptions,
                    out IntPtr ppIDecoder);
                void CreateDecoderFromFileHandleDummy();
                void CreateComponentInfoDummy();
                void CreateDecoderDummy();
                void CreateEncoderDummy();
                void CreatePaletteDummy();
                int CreateFormatConverter(out IntPtr ppIFormatConverter);
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
                if (result != S_OK)
                    throw new Exception("GetFrame failed");
                return new BitmapFrameDecode(nativePointer);
            }

            [Guid("9EDDE9E7-8DEE-47ea-99DF-E6FAF2ED44BF")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            [ComImport]
            interface IWICBitmapDecoder
            {
                void QueryCapabilityDummy();
                void InitializeDummy();
                void GetContainerFormatDummy();
                void GetDecoderInfoDummy();
                void CopyPaletteDummy();
                void GetMetadataQueryReaderDummy();
                void GetPreviewDummy();
                void GetColorContextsDummy();
                void GetThumbnailDummy();
                void GetFrameCountDummy();
                int GetFrame(uint index, out IntPtr ppIFrameDecode);
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

            [Guid("00000120-a8f2-4877-ba0a-fd2b6645fb94")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            [ComImport]
            public interface IWICBitmapSource
            {
                void GetSize(out uint puiWidth, out uint puiHeight);
                void GetPixelFormat(out Guid pPixelFormat);
                void GetResolutionDummy();
                void CopyPaletteDummy();
                void CopyPixels(
                    IntPtr prc, // WICRect
                    uint cbStride,
                    uint cbBufferSize,
                    IntPtr pbBuffer);
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

            [Guid("00000301-a8f2-4877-ba0a-fd2b6645fb94")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            [ComImport]
            public interface IWICFormatConverter
            {
                #region IWICBitmapSource
                void GetSizeDummy();
                void GetPixelFormatDummy();
                void GetResolutionDummy();
                void CopyPaletteDummy();
                void CopyPixelsDummy();
                #endregion
                void Initialize(
                    IntPtr pISource,
                    [MarshalAs(UnmanagedType.LPStruct)]
                    Guid dstFormat,
                    uint dither,
                    IntPtr pIPalette,
                    double alphaThresholdPercent,
                    uint paletteTranslate);
            };
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
