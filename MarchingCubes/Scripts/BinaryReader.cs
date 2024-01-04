public static class BinaryReader {

    public const int WIDTH = 256;
    public const int HEIGHT = 256;
    
    public static short[,] GetShortArrayFromFile(string path)
    {
        var array = new short[WIDTH, HEIGHT];
        var bytesRead = System.IO.File.ReadAllBytes(path);
        
        // Read 16-bit integers from the file into the array from Mac byte order to Windows byte order
        for (var i = 0; i < bytesRead.Length; i += 2)
        {
            var b1 = bytesRead[i];  
            var b2 = bytesRead[i + 1];
            array[i / 2 % WIDTH, i / 2 / WIDTH] = (short) (b1 << 8 | b2);
        }

        return array;
    }
}   
