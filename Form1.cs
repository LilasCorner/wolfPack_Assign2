using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wolfPack_Assign2
{
    public partial class Form1 : Form
    {
        protected static SortedDictionary<uint, String> usersMap = new SortedDictionary<uint, String>();
        protected static SortedDictionary<uint, String> subMap = new SortedDictionary<uint, String>();
        protected static SortedDictionary<uint, String> postMap = new SortedDictionary<uint, String>();
        protected static SortedDictionary<uint, String> comMap = new SortedDictionary<uint, String>();

        public Form1()
        {
            InitializeComponent();
            readData();
            addData();
        }


        public void readData()
        {
            string lineRead = "";

            #region

            //validate users.txt exists!
            if (File.Exists("..//..//users.txt"))
            {
                //reading users.txt
                using (StreamReader inFile = new StreamReader("..//..//users.txt"))
                {
                    lineRead = inFile.ReadLine(); // prime read

                    while (lineRead != null)
                    {

                        string[] tokens = lineRead.Split('\t');

                        //parse all tokens into dictionary + array of all ID's
                        usersMap.Add(Convert.ToUInt32(tokens[0]), lineRead);


                        lineRead = inFile.ReadLine();
                    }
                }
            }

            #endregion


            #region

            //validate users.txt exists!
            if (File.Exists("..//..//subreddits.txt"))
            {
                //reading users.txt
                using (StreamReader inFile = new StreamReader("..//..//subreddits.txt"))
                {
                    lineRead = inFile.ReadLine(); // prime read

                    while (lineRead != null)
                    {

                        string[] tokens = lineRead.Split('\t');

                        //parse all tokens into dictionary + array of all ID's
                        subMap.Add(Convert.ToUInt32(tokens[0]), lineRead);


                        lineRead = inFile.ReadLine();
                    }
                }
            }

            #endregion


            #region

            //validate users.txt exists!
            if (File.Exists("..//..//posts.txt"))
            {
                //reading users.txt
                using (StreamReader inFile = new StreamReader("..//..//posts.txt"))
                {
                    lineRead = inFile.ReadLine(); // prime read

                    while (lineRead != null)
                    {

                        string[] tokens = lineRead.Split('\t');

                        //parse all tokens into dictionary + array of all ID's
                        postMap.Add(Convert.ToUInt32(tokens[1]), lineRead);


                        lineRead = inFile.ReadLine();
                    }
                }
            }

            #endregion


            #region

            //validate users.txt exists!
            if (File.Exists("..//..//comments.txt"))
            {
                //reading users.txt
                using (StreamReader inFile = new StreamReader("..//..//comments.txt"))
                {
                    lineRead = inFile.ReadLine(); // prime read

                    while (lineRead != null)
                    {

                        string[] tokens = lineRead.Split('\t');

                        //parse all tokens into dictionary + array of all ID's
                        comMap.Add(Convert.ToUInt32(tokens[0]), lineRead);


                        lineRead = inFile.ReadLine();
                    }
                }
            }

            #endregion

        }

        public void addData()
        {

            foreach (var item in usersMap.Keys)
            {
                userNameCombo.Items.Add(usersMap[item]);
            }

            foreach (var item in subMap.Keys)
            {
                subredditListBox.Items.Add(subMap[item]);
            }

            foreach (var item in postMap.Keys)
            {
                postListBox.Items.Add(postMap[item]);
            }

            foreach (var item in comMap.Keys)
            {
                commentListBox.Items.Add(comMap[item]);
            }


        }
    }
 }





