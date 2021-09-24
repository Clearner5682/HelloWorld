/*各种文件和流的读写方法*/


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Utils
{
    public class FileHelper
    {
        // 将一个文件流保存成文件最优雅的方式是CopyTo和CopyToAsync
        public void CopyToByFile(string copyFile,string saveFile)
        {
            using (var copyStream=File.OpenRead(AppDomain.CurrentDomain.BaseDirectory+"\\Files\\"+copyFile))
            {
                using (var saveStream = File.Open(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\"+saveFile,FileMode.Create))
                {
                    copyStream.Seek(0, SeekOrigin.Begin);
                    copyStream.CopyTo(saveStream);
                }
            }
        }

        public async Task CopyToByFileAsync(string copyFile, string saveFile)
        {
            using (var copyStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + copyFile))
            {
                using (var saveStream = File.Open(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + saveFile, FileMode.Create))
                {
                    copyStream.Seek(0, SeekOrigin.Begin);
                    await copyStream.CopyToAsync(saveStream);
                }
            }
        }

        // 其次的方法就是自己控制在内存中复制
        public void CopyBySelf(string copyFile,string saveFile)
        {
            using (var copyStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + copyFile))
            {
                using (var saveStream = File.Open(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + saveFile, FileMode.Create))
                {
                    copyStream.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[1024]; // 建立一个1K大小的缓冲区
                    int length;
                    while ((length = copyStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        saveStream.Write(buffer, 0, length);
                    }
                }
            }
        }

        // 不太推荐这样用，复制1G的文件，会占用2G的内存
        // 读取1G文件占用1G内存，内存中复制又会占用1G
        // 但是用起来很方便
        public void CopyByMemory(string copyFile,string saveFile)
        {
            using (var copyStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + copyFile))
            {
                using (var saveStream=new MemoryStream())
                {
                    copyStream.CopyTo(saveStream);
                    byte[] bytes = saveStream.ToArray();
                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + saveFile, bytes);
                }
            }
        }

        // 和CopyByMemory一样，都是在申请一个大的缓存，一次性复制流
        public void CopyByStream(string copyFile,string saveFile)
        {
            using (var copyStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + copyFile))
            {
                using (var saveStream = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + saveFile,(int)copyStream.Length))
                {
                    byte[] buffer = new byte[copyStream.Length];
                    copyStream.Read(buffer, 0, buffer.Length);
                    saveStream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        // 这是一个超级慢的方法，一个字节一个字节的写入
        public void CopyByByte(string copyFile,string saveFile)
        {
            using (var copyStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + copyFile))
            {
                using (var saveStream = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + saveFile))
                {
                    int byteInt;
                    while((byteInt = copyStream.ReadByte()) != -1)// -1代表End，只要读取到的不是-1就写入到saveStream
                    {
                        saveStream.WriteByte((byte)byteInt);
                    }
                }
            }
        }
    }
}
