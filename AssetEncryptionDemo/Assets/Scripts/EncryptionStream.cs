using System.IO;

public class EncryptionStream : FileStream {
    public byte[] EncryKey { get; private set; }

    // 0x6F7A81F3
    // 0110 1111 0111 1010 1000 0001 1111 0011
    //                               0110 1111 0111 1010 1000 0001 1111 0011  >> 0
    //                     0000 0000 0110 1111 0111 1010 1000 0001  >> 8
    //           0000 0000 0000 0000 0110 1111 0111 1010  >> 16
    // 0000 0000 0000 0000 0000 0000 0110 1111  >> 24
    public static byte[] UInt32ToByte4( uint vl ) {
        byte[] ret = new byte[4];
        ret[0] = (byte)( vl >> 0 );
        ret[1] = (byte)( vl >> 8 );
        ret[2] = (byte)( vl >> 16 );
        ret[3] = (byte)( vl >> 24 );
        return ret;
    }

    public EncryptionStream( string path, FileMode mode, uint encryKey )
        : base( path, mode ) { EncryKey = UInt32ToByte4( encryKey ); }
    public EncryptionStream( string path, FileMode mode, FileAccess access, uint encryKey )
        : base( path, mode, access ) { EncryKey = UInt32ToByte4( encryKey ); }
    public EncryptionStream( string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, uint encryKey )
        : base( path, mode, access, share, bufferSize ) { EncryKey = UInt32ToByte4( encryKey ); }
    public EncryptionStream( string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync, uint encryKey )
        : base( path, mode, access, share, bufferSize, useAsync ) { EncryKey = UInt32ToByte4( encryKey ); }

    // byte[] 1 2 5 10 29
    // 0110 1111 0111 1010 1000 0001 1111 001[0] ^ 0110 1111 0111 1010 1000 0001 1111 0011
    // 前边原本没数据的部分没动，那就是一样，又变成0
    // 0000 0000 0000 0000
    // 后边的 001[0] ^ 0011 = 0001
    // 异或 把原本的1取出来了

    public override int Read( byte[] array, int offset, int count ) {
        int ret = base.Read( array, offset, count );
        for( int i = 0; i < ret; ++i ) {
            array[i] ^= EncryKey[i % 4];
        }
        return ret;
    }

    public override bool CanSeek => true;
}