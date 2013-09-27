using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    internal static class NativeMethods
    {
        public const int S_OK = 0;

        public static readonly Guid CLSID_WICImagingFactory = new Guid("cacaf262-9370-4615-a13b-9f5539da4c0a");

        public static readonly Guid WICPixelFormat1bppIndexed = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc901");
        public static readonly Guid WICPixelFormatBlackWhite = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc905");
        public static readonly Guid WICPixelFormat8bppIndexed = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc904");
        public static readonly Guid WICPixelFormatDontCare = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc900");
        public static readonly Guid WICPixelFormat24bppBGR = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc90c");
        public static readonly Guid WICPixelFormat32bppBGRA = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc90e");
        public static readonly Guid WICPixelFormat48bppRGB = new Guid("6fddc324-4e03-4bfe-b185-3d77768dc915");

        [DllImport("ole32.dll", ExactSpelling = true, EntryPoint = "CoCreateInstanceFromApp", PreserveSig = true)]
        public static extern int CoCreateInstanceFromApp(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
            IntPtr pUnkOuter,
            CLSCTX dwClsContext,
            IntPtr reserved,
            int countMultiQuery,
            ref MultiQueryInterface query);
    }

    [Flags]
    internal enum CLSCTX : uint
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

    internal enum WICDecodeOptions : uint
    {
        WICDecodeMetadataCacheOnDemand = 0,
        WICDecodeMetadataCacheOnLoad = 1,
        WICMETADATACACHEOPTION_FORCE_DWORD = 0x7fffffff
    }

    [Guid("ec5ec8a9-c395-4314-9c77-54d7a935ff70")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IWICImagingFactory
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

    [Guid("9EDDE9E7-8DEE-47ea-99DF-E6FAF2ED44BF")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IWICBitmapDecoder
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

    [Guid("00000120-a8f2-4877-ba0a-fd2b6645fb94")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IWICBitmapSource
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

    [Guid("00000301-a8f2-4877-ba0a-fd2b6645fb94")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IWICFormatConverter
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
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MultiQueryInterface
    {
        public IntPtr InterfaceIID;
        public IntPtr IUnknownPointer;
        public int ResultCode;
    }
}
