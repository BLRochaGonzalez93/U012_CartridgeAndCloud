using System;
using System.IO;
using System.Text;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    internal static class AtomicJsonFile
    {
        public static void Write(
            string path,
            string content)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(
                    "A path is required.",
                    nameof(path));
            }

            string directory =
                Path.GetDirectoryName(path);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string temporary = path + ".tmp";
            byte[] bytes =
                Encoding.UTF8.GetBytes(
                    content ?? string.Empty);

            using (FileStream stream =
                   new FileStream(
                       temporary,
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                stream.Write(
                    bytes,
                    0,
                    bytes.Length);
                stream.Flush(true);
            }

            if (File.Exists(path))
            {
                string backup = path + ".bak";

                if (File.Exists(backup))
                {
                    File.Delete(backup);
                }

                try
                {
                    File.Replace(
                        temporary,
                        path,
                        backup);
                }
                catch (PlatformNotSupportedException)
                {
                    File.Copy(
                        path,
                        backup,
                        true);
                    File.Delete(path);
                    File.Move(temporary, path);
                }
            }
            else
            {
                File.Move(temporary, path);
            }
        }
    }
}
