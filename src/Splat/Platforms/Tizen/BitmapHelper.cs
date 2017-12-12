namespace Splat
{
    public class BitmapHelper
    {
        /*
            Since tizen does not have a constructor that creates an empty image object, 
            it creates an empty image with the size of 100*100 pixels below when creating the default constructor.
        */
        public static byte[] EmptyImageBinary
        {
            get
            {               
                return new byte[] {
                    137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 100, 0, 0, 0, 100, 8, 2, 0, 0, 0, 255, 128, 2, 3, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4,
                    103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 195, 0, 0, 14, 195, 1, 199, 111, 168, 100, 0, 0, 0, 52, 73, 68, 65, 84, 120, 94, 237, 193, 1, 13, 0, 0, 0,
                    194, 160, 247, 79, 109, 14, 55, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128, 27, 53, 117, 148, 0, 1, 4, 253, 190, 98, 0, 0, 0,
                    0, 73, 69, 78, 68, 174, 66, 96, 130
                };
            }
        }
    }
}
