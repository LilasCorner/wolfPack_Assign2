using System;
using System.Collections;
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
        protected static SortedDictionary<uint, User> usersMap = new SortedDictionary<uint, User>();
        protected static SortedDictionary<uint, Subreddit> subMap = new SortedDictionary<uint, Subreddit>();
        protected static SortedDictionary<uint, String> postMap = new SortedDictionary<uint, String>();
        protected static SortedDictionary<uint, String> comMap = new SortedDictionary<uint, String>();
        public static ArrayList globalIds = new ArrayList();

        public const int GOOD_EXIT = 1;
        public const int SUB_INDEX = 4;
        public const int POST_INDEX = 14;
        public const int COM_INDEX = 12;
        public const int USER_INDEX = 6;

        protected static string selectedUser = "";
        protected static string selectedSub = "";


        enum badWords
        {
            fudge,
            shoot,
            baddie,
            butthead
        }


        public Form1()
        {
            InitializeComponent();
            readData();
            addData();
        }

        //FIX LATER NEED DOC BOX
        //FIX LATER NEEDS IMPLEMENTATION FOR POSTS CLASS DICTIONARY, COMMENT CLASS DICTIONARY
        // right now it just reads the string into the dictionary, it does not actually create the object
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
                        uint id = Convert.ToUInt32(tokens[0]);

                        //parse all tokens into dictionary + array of all ID's
                        usersMap[id] = new User(tokens);
                        globalIds.Add(id);

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
                        subMap[Convert.ToUInt32(tokens[0])] = new Subreddit(tokens);
                        globalIds.Add(Convert.ToUInt32(tokens[0]));

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

        
        // Method:  addData()
        // Purpose: adds the data from the Dictionaries to their components on the form
        // Params: N/A
        // Returns: N/A
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


        }

        //FIX LATER NEED DOC BOX
        public uint nameToId(string name, int dictionary)
        {
            if (dictionary == 1)
            {

                foreach (var item in usersMap.Keys)
                {
                    if (usersMap[item].Name.Equals(name))
                    {
                        return item;
                    }
                }

            }
            else if (dictionary == 2 )
            {
                foreach (var item in subMap.Keys)
                {
                    if (subMap[item].Name.Equals(name))
                    {
                        return item;
                    }
                }
            }
            

            return 0;
        }

        //returns what class type the paramater ID is from FIX LATER NEEDS DOC BOX
        public static int whatAmI(uint id)
        {
            if (usersMap.ContainsKey(id)) //USER
            {
                return 0;
            }
            else if (subMap.ContainsKey(id)) // SUBREDDIT
            {
                return 1;
            }
            else if (postMap.ContainsKey(id)) //POST
            {
                return 2;
            }

            return 3; //COMMENT

        }

        // Method:  UniqueId(uint id)
        // Purpose: verifies given ID is completely unique across all classes
        // Params: uint id, the id we want to verify
        // Returns: true if id is unique, false if it is duplicate
        static public bool UniqueId(uint id)
        {
            //sort globalId array
            globalIds.Sort();
            int flag = globalIds.BinarySearch(id);

            //binary search array for duplicates -- if we got -1, id is good, positive number is bad!
            if (flag > 0)
            {
                return false;
            }

            return true;
        }


        // Method:  genId()
        // Purpose: generate completely unique ID if needed
        // Params: N/A
        // Returns: uint of new ID
        static public uint genId()
        {
            //create random num 0-9999
            Random random = new Random();

            uint id = Convert.ToUInt32(random.Next(0, 10000));

            //if id generated is invalid, loop until we generate a valid one
            while (UniqueId(id) == false)
            {
                id = Convert.ToUInt32(random.Next(0, 10000));
            }

            globalIds.Add(id);
            return id;
        }


        // Method:  validateName(string name)
        // Purpose: verifies given name is within criteria for reddit names
        // Params: string name, the name we're going to check
        // Returns: true if name is valid, false if it's bad
        public static bool validateName(string name, int min, int max)
        {
            //both these will = true if white space located, we want false value ideally
            bool first = char.IsWhiteSpace(name[0]);
            bool last = char.IsWhiteSpace(name[name.Length - 1]);

            //perform validation of name has to be char >5 && <21 && cannot begin/end with space characters

            if (name.Length < min || name.Length > max || first || last || findBadWords(name))
            {
                return false;
            }

            return true;
        }


        // Method:  findBadWords(string str)
        // Purpose: checks if given string has a bad word
        // Params: string str, the string we're going to check
        // Returns: throws exception if string has badword, false if no badword found
        public static bool findBadWords(string str)
        {
            try
            {
                foreach (string word in Enum.GetNames(typeof(badWords)))
                {
                    if (str.Contains(word))
                    {

                        throw new FoulLanguageException();

                    }

                }

            }
            catch (FoulLanguageException e)
            {
                Console.WriteLine(e.ToString());

            }

            return false;
        }

        //FIX LATER NEED DOC BOX
        private void userNameCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userNameCombo.SelectedIndex != -1) //no username selected
            {
                selectedUser = userNameCombo.Items[userNameCombo.SelectedIndex].ToString();
            }
        }

        //FIX LATER NEED DOC BOX
        //FIX LATER NEED TO IMPLEMENT COMMENTS CLASS 
        public void populateComments()
        {
            //aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
        }

        public bool loginCheck(string user, string pass)
        {

            string[] users = user.Split(' ');
            MessageBox.Show(user);

            uint id = nameToId(users[0], 1);
            MessageBox.Show(id.ToString());

            string attempt = pass.GetHashCode().ToString("X");
            MessageBox.Show(attempt);
            string correct = usersMap[id].PassHash;
            MessageBox.Show(correct);


            if (correct == attempt)
            {
                return true;
            }
            return false;
        }


        //FIX LATER NEED DOC BOX
        private void loginButton_Click(object sender, EventArgs e)
        {
            
            if(userNameCombo.SelectedIndex == -1) //no username selected
            {
                sysOutputTextBox.AppendText("Please select a user and type their password.");
                sysOutputTextBox.AppendText(Environment.NewLine);
            }
            else if (String.IsNullOrEmpty(passwordTextBox.Text)) //no password typed
            {
                string[] user = selectedUser.Split(' ');
                sysOutputTextBox.AppendText("Please type the password for user "+ user[0] + ".");
                sysOutputTextBox.AppendText(Environment.NewLine);
            }
            else if (loginCheck(selectedUser, passwordTextBox.Text)) //try verify login details
            {
                sysOutputTextBox.AppendText("Login successful.");
                sysOutputTextBox.AppendText(Environment.NewLine);
                populateComments();
            }
            else //failed password
            {
                string[] user = selectedUser.Split(' ');
                sysOutputTextBox.AppendText("Invalid password for user: " + user[0] + "  - Action Failed.");
                sysOutputTextBox.AppendText(Environment.NewLine); 
                loginButton.Text = "Retry Password";
            }
        }

        private void subredditListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(subredditListBox.SelectedIndex != -1)
            {
                selectedSub = subredditListBox.Items[subredditListBox.SelectedIndex].ToString();
                uint index = nameToId(selectedSub, 2);
                memberLabel.Text = subMap[index].Members.ToString();
                memberLabel.Visible = true;
                activeLabel.Text = subMap[index].Active.ToString();
                activeLabel.Visible = true;
            }

        }
    }
}





