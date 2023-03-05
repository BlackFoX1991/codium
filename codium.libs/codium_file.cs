using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codium_extension_template
{
    public class codium_file
    {

        public enum modus_
        {
            NULL,
            file_read,
            file_write
        }

        public StreamReader freader { get; set; } = null!;
        public StreamWriter fwriter { get; set; } = null!;

        public modus_ MODE = modus_.NULL;


        public codium_file(string filename, modus_ file_open_mode = modus_.NULL)
        {
            if ((int)file_open_mode < 1 || (int)file_open_mode > 2) throw new Exception("Invalid File Mode");

            switch (file_open_mode)
            {
                case modus_.file_read:
                    freader = new StreamReader(filename);
                    break;
                case modus_.file_write:
                    fwriter = new StreamWriter(filename);
                    break;


            }
        }

    }
}
