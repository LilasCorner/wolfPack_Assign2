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
        protected static string selectedUser = "";
        protected static string selectedSub = "";
        protected static string selectedPost = "";
        protected static string selectedCom = "";

        public static SortedDictionary<uint, User> usersMap = new SortedDictionary<uint, User>();
        public static SortedDictionary<uint, Subreddit> subMap = new SortedDictionary<uint, Subreddit>();
        public static SortedDictionary<uint, Post> postMap = new SortedDictionary<uint, Post>();
        public static SortedDictionary<uint, Comment> comMap = new SortedDictionary<uint, Comment>();
        public static ArrayList globalIds = new ArrayList();

        public const int SUB_INDEX = 4;
        public const int POST_INDEX = 15;
        public const int COM_INDEX = 12;
        public const int USER_INDEX = 6;

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
                        uint id = Convert.ToUInt32(tokens[1]);

                        //parse all tokens into dictionary + array of all ID's
                        postMap[id] = new Post(tokens) ;
                        globalIds.Add(id);

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
                    uint id = 0;
                    uint parent = 0;

                    while (lineRead != null)
                    {

                        string[] tokens = lineRead.Split('\t');


                        id = Convert.ToUInt32(tokens[COM_INDEX - 12]);
                        parent = Convert.ToUInt32(tokens[COM_INDEX - 9]);
                        int what = whatAmI(parent);
                        Comment newCom = new Comment(tokens);
                        comMap[id] = newCom;


                        //parse all tokens into dictionary + array of all ID's
                        if (what == 2)//post
                        {
                            postMap[parent].addComment(newCom);
                        }
                        else if (what == 3)//comment reply
                        {
                            foreach (var item in postMap.Keys)
                            {
                                foreach (var index in postMap[item].PostComments)
                                {
                                    if (index.Id == parent)
                                    {
                                        index.addComment(newCom);
                                    }
                                }
                            }

                        }



                        globalIds.Add(id);


                        //read next line
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

            foreach (KeyValuePair<uint, Subreddit> item in subMap.OrderBy(key => key.Value))
            {
                subredditListBox.Items.Add(item.Value);
            }


        }

        //FIX LATER NEED DOC BOX
        public uint nameToId(string name, uint dictionary)
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
            
            foreach (string word in Enum.GetNames(typeof(badWords)))
            {
                if (str.Contains(word))
                {

                    throw new FoulLanguageException();

                }

            }

            return false;
        }

        //FIX LATER NEED DOC BOX
        private void userNameCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            passwordTextBox.ReadOnly = false;
            passwordTextBox.Clear();

            if (userNameCombo.SelectedIndex != -1) //username selected
            {
                selectedUser = userNameCombo.Items[userNameCombo.SelectedIndex].ToString();
                sysOutputTextBox.AppendText("Please login.");
                sysOutputTextBox.AppendText(Environment.NewLine);
                clearListBoxes();
            }
        }

        //FIX LATER NEED DOC BOX
        public void clearListBoxes()
        {
            commentListBox.Items.Clear();
            postListBox.Items.Clear();
        }

        //FIX LATER NEED DOC BOX
        public void populateComments(string parentName, uint map)
        {
            uint parent = nameToId(parentName, map);

            foreach (KeyValuePair<uint, Comment> item in comMap.OrderBy(key => key.Value).Reverse())
            {
                if (item.Value.AuthorId == parent)
                {
                    commentListBox.Items.Add(item.Value.ToString());
                    commentListBox.Items.Add(Environment.NewLine);
                }
                
            }

            if (commentListBox.Items.Count == 0)//if empty, give user feedback
            {
                commentListBox.Items.Add("Wow, such empty!");
            }

        }

        //FIX LATER NEED DOC BOX
        //FIX LATER NEED TO IMPLEMENT COMMENTS CLASS 
        public void populatePostComments(uint _id)
        {
            
          
            foreach (KeyValuePair<uint, Comment> item in comMap.OrderBy(key => key.Value).Reverse())
            {

                if (item.Value.ParentId == _id && comMap.ContainsKey(item.Key))
                {
                    commentListBox.Items.Add(item.Value.ToString());
                    commentListBox.Items.Add(Environment.NewLine);

                    foreach(var index in item.Value.CommentReplies.OrderBy(key => key.Score).Reverse())
                    {

                        commentListBox.Items.Add("\t"+index.ToString());
                        commentListBox.Items.Add(Environment.NewLine);
                        
                        foreach(var last in index.CommentReplies)
                        {

                            commentListBox.Items.Add("\t\t" + last.ToString());
                            commentListBox.Items.Add(Environment.NewLine);
                        }
                    }
                }
                
            }
        
            if (commentListBox.Items.Count == 0)//if empty, give user feedback
            {
                commentListBox.Items.Add("Wow, such empty!");
            }

        }

        //FIX LATER NEED DOC BOX
        public bool loginCheck(string user, string pass)
        {

            string[] users = user.Split(' ');
            uint id = nameToId(users[0], 1);
            string attempt = pass.GetHashCode().ToString("X");
            string correct = usersMap[id].PassHash;


            if (correct == attempt)
            {
                return true;
            }
            return false;
        }

        //FIX LATER NEED DOC BOX
        private void loginButton_Click(object sender, EventArgs e)
        {
            clearListBoxes(); 
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
                string[] user = selectedUser.Split(' ');

                sysOutputTextBox.AppendText("Login successful. Hello " + user[0] + "!");
                sysOutputTextBox.AppendText(Environment.NewLine);
                sysOutputTextBox.AppendText("Displaying posts and comments for user " + user[0] + ".");
                sysOutputTextBox.AppendText(Environment.NewLine);
                passwordTextBox.ReadOnly = true;
                loginButton.Text = "Login";
                populatePosts(user[0], 1);
                //uint parentId = nameToId(user[0], 1);
                populateComments(user[0],1);
            }
            else //failed password
            {
                string[] user = selectedUser.Split(' ');
                sysOutputTextBox.AppendText("Invalid password for user: " + user[0] + "  - Action Failed.");
                sysOutputTextBox.AppendText(Environment.NewLine); 
                loginButton.Text = "Retry Password";
                passwordTextBox.Clear();
            }
        }

        //FIX LATER  doc box
        private void populatePosts(string parentName, uint map)
        {
            uint parentId = nameToId(parentName, map);

            

            if(subMap.ContainsKey(parentId)) // if we're populating posts from a subreddit
            { 
                if (subMap[parentId].Name == "all") // if all subreddit selected
                {
                    foreach(KeyValuePair<uint, Post> index in postMap.OrderBy(key => key.Value).Reverse()) 
                    {
                        postListBox.Items.Add(index.Value.ToStringShort());
                    }
                }
                else
                {
                    foreach (KeyValuePair<uint, Post> index in postMap.OrderBy(key => key.Value).Reverse()) 
                    {
                        if(index.Value.AuthorId == parentId || index.Value.SubHome == parentId)
                        {
                            postListBox.Items.Add(index.Value.ToStringShort());
                        }
                    }
                }
               
            }
            else //we're populating from a user's posts
            {
                foreach (KeyValuePair<uint, Post> index in postMap.OrderBy(key => key.Value).Reverse())
                {
                    if (index.Value.AuthorId == parentId)
                    {
                        postListBox.Items.Add(index.Value.ToStringShort());
                    }
                }
            }

            if (postListBox.Items.Count == 0)
            {
                postListBox.Items.Add("Wow, such empty!");
            }
        }

        //FIX LATER NEED DOC BOX
        private void subredditListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(subredditListBox.SelectedIndex != -1)
            {
                clearListBoxes();
                selectedSub = subredditListBox.Items[subredditListBox.SelectedIndex].ToString();
                uint index = nameToId(selectedSub, 2);

                if(subMap[index].Name != "all")
                {
                    memberLabel.Text = subMap[index].Members.ToString();
                    memberLabel.Visible = true;
                    activeLabel.Text = subMap[index].Active.ToString();
                    activeLabel.Visible = true;
                }
                else
                {
                    memberLabel.Visible = false;
                    activeLabel.Visible = false;
                }
                
            }
            populatePosts(selectedSub,2);
            

        }

        //FIX LATER NEED DOC BOX
        private void postListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (postListBox.SelectedIndex != -1 && postListBox.Items[postListBox.SelectedIndex].ToString() != "Wow, such empty!")
            {
                commentListBox.Items.Clear();
                commentListBox.ClearSelected();

                selectedPost = postListBox.Items[postListBox.SelectedIndex].ToString();

                uint _id = Convert.ToUInt32(selectedPost.Substring(1, 4));
                sysOutputTextBox.AppendText(postMap[_id].ToString());
                sysOutputTextBox.AppendText(Environment.NewLine);
                populatePostComments(_id);

                if (postMap[_id].Locked)
                {
                    sysOutputTextBox.AppendText( "Replies disabled for locked posts.");
                    sysOutputTextBox.AppendText(Environment.NewLine);
                    addReplyTextBox.Text = "Replies disabled for locked posts.";
                    addReplyTextBox.ReadOnly = true;
                }
                else
                {
                    addReplyTextBox.Clear();
                    addReplyTextBox.ReadOnly = false;
                }
            }
            
        }

        //FIX LATER NEED DOC BOX
        private void deletePostButton_Click(object sender, EventArgs e)
        {
            if(postListBox.SelectedIndex != -1 && postListBox.Items[postListBox.SelectedIndex].ToString() != "Wow, such empty!")
            {

                selectedPost = postListBox.Items[postListBox.SelectedIndex].ToString();
                string[] users = selectedUser.Split(' ');

                uint user = nameToId(users[0],1);
                uint _id = Convert.ToUInt32(selectedPost.Substring(1, 4));

                if(selectedUser != "")
                {
                     if(postMap[_id].AuthorId == user || usersMap[user].UserType == 2)
                    {
                        postListBox.Items.RemoveAt(postListBox.SelectedIndex);
                        postMap.Remove(_id);

                        sysOutputTextBox.AppendText("Post successfully deleted!");
                        sysOutputTextBox.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        sysOutputTextBox.AppendText("You cannot delete other user's posts");
                        sysOutputTextBox.AppendText(Environment.NewLine);
                    }
                }
                else
                {
                    sysOutputTextBox.AppendText("Please log in to delete a post");
                    sysOutputTextBox.AppendText(Environment.NewLine);
                }

               

               
            }
        }

        //FIX LATER NEED DOC BOX
        private void deleteCommentButton_Click(object sender, EventArgs e)
        {

            if (commentListBox.SelectedIndex != -1 && commentListBox.Items[commentListBox.SelectedIndex].ToString() != "Wow, such empty!" && selectedUser != "")
            {
                string trimmed = String.Concat(commentListBox.Items[commentListBox.SelectedIndex].ToString().Where(c => !Char.IsWhiteSpace(c)));
                string selectedCom = trimmed;

                uint _id = Convert.ToUInt32(selectedCom.Substring(1, 4));
                
                uint userId = comMap[_id].AuthorId;

                string[] user = selectedUser.Split(' ');
                uint currentUser = nameToId(user[0], 1);

                MessageBox.Show(usersMap[currentUser].Name);
                uint deleteItem =0;
                Comment deleteIndex = new Comment();
                bool replyFlag = false;

                if (usersMap[currentUser].Name.Equals(usersMap[userId].Name) || usersMap[currentUser].UserType == 2)
                {
                    
                    foreach(var item in comMap.Keys) //delete the comment from any other replies if it is a reply
                    {
                        foreach(var index in comMap[item].CommentReplies)
                        {
                            if (index.Id == _id) { 
                                deleteItem=item;
                                deleteIndex=index;
                                replyFlag = true;
                            }
                        }
                        
                    }

                    if (replyFlag) { 
                         comMap[deleteItem].CommentReplies.Remove(deleteIndex);
                    }

                    replyFlag=false;//resetting flag
                    comMap.Remove(_id);// then we remove from the map entirely
                    commentListBox.Items.RemoveAt(commentListBox.SelectedIndex); // remove from the list box



                    uint postId = Convert.ToUInt32(selectedPost.Substring(1,4)); //remove from the post it's from                  
                    foreach(var items in postMap.Keys) { 

                        foreach(var index in postMap[items].PostComments)
                        {
                            if (index.Id == _id) { 
                                deleteItem = items;
                                deleteIndex = index;
                                replyFlag = true;
                            }
                        }
                    }

                    if (replyFlag) { 
                        postMap[deleteItem].PostComments.Remove(deleteIndex);
                    }

                    string temp = String.Concat(selectedCom.Where(c => !Char.IsWhiteSpace(c)));
                    uint comId = Convert.ToUInt32(temp.Substring(1, 4)); //get actual id no whitespace

                    if (comMap.ContainsKey(_id)) { 
                        MessageBox.Show("Literally how");
                    }




                    sysOutputTextBox.AppendText("Comment successfully deleted!");
                    sysOutputTextBox.AppendText(Environment.NewLine);
                }
                else
                {
                    sysOutputTextBox.AppendText("You may not delete a comment you didn't write");
                    sysOutputTextBox.AppendText(Environment.NewLine);
                }
            }
            else
            {
                  sysOutputTextBox.AppendText("Please select a comment, and verify you are logged in");
                  sysOutputTextBox.AppendText(Environment.NewLine);
            }
        }

        //FIX LATER NEED DOC BOX
        private void addReplyButton_Click(object sender, EventArgs e)
        {
            if(addReplyTextBox.Text == "" || postListBox.SelectedIndex == -1 || postListBox.Items[postListBox.SelectedIndex].ToString() == "Wow, such empty!")

            {
                sysOutputTextBox.AppendText("Please type the reply you would like to add and select either a post or comment");
                sysOutputTextBox.AppendText(Environment.NewLine);
            }
            else
            {

                if ( selectedUser != "")
                {
                    try
                    {
                        string selectedMessage = "";
                        uint parent = 0;
                        bool commentFlag = false; //false if its for a post, true if its for a post
                        string response = addReplyTextBox.Text; // get reply content
                        string[] users = selectedUser.Split(' ');
                        uint author = nameToId(users[0], 1); //get user's id


                        if (commentListBox.SelectedIndex != -1 && commentListBox.Items[commentListBox.SelectedIndex].ToString() != "Wow, such empty!")//its a comment
                        {
                            selectedMessage = commentListBox.Items[commentListBox.SelectedIndex].ToString();
                            string trimmed = String.Concat(selectedMessage.Where(c => !Char.IsWhiteSpace(c)));
                            parent = Convert.ToUInt32(trimmed.Substring(1, 4)); //get parent id
                            commentFlag = true;
                        }
                        else // its a post
                        {
                            selectedMessage = postListBox.Items[postListBox.SelectedIndex].ToString();
                            parent = Convert.ToUInt32(selectedPost.Substring(1, 4)); //get parent id
                        }


                        findBadWords(response);


                        Comment temp = new Comment(response, author, parent);
                        comMap[temp.Id] = temp;


                        if (!commentFlag)//its a post
                        {
                            foreach (var item in postMap.Keys)
                            {
                                
                                if (postMap[item].Id == parent)
                                {
                                    postMap[item].PostComments.Add(temp);
                                }
                                
                            }
                        }
                        else //its a comment
                        {
                            foreach (var item in comMap.Keys)
                            {
                                
                                if (comMap[item].Id == parent)
                                {
                                    comMap[item].addComment(temp);
                                }
                                
                            }
                        }
                        commentListBox.Items.Clear();//clearing the comment box
                        populatePostComments(Convert.ToUInt32(selectedPost.Substring(1, 4))); //repopulating it
                        sysOutputTextBox.AppendText("Comment added successfully!");
                        sysOutputTextBox.AppendText(Environment.NewLine);
                        addReplyTextBox.Clear();

                    }
                    catch (FoulLanguageException a)
                    {
                        MessageBox.Show(a.ToString()); //KEEP THIS MESSAGE BOX!
                        sysOutputTextBox.AppendText(a.ToString());
                        sysOutputTextBox.AppendText(Environment.NewLine);
                        addReplyTextBox.Clear();

                    }


                }
                else
                {
                    sysOutputTextBox.AppendText("Please verify you are properly logged in.");
                    sysOutputTextBox.AppendText(Environment.NewLine);
                }
            }



        }

       
    }
    
}





