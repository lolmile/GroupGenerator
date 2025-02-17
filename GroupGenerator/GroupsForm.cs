namespace GroupGenerator
{
    using System.ComponentModel;
    using ExtensionMethods;

    /// <summary>
    /// Main form class.
    /// </summary>
    public partial class GroupsForm : Form
    {
        /// <summary>
        /// List of people used to make groups.
        /// </summary>
        /// <remarks>
        /// This list is bound to the ListBox displaying the people in the groups form.
        /// </remarks>
        private BindingList<Person> personList;

        /// <summary>
        /// Random number generator for picking.
        /// </summary>
        private Random rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupsForm"/> class.
        /// Constructor of the main form.
        /// </summary>
        public GroupsForm()
        {
            this.InitializeComponent();
            this.personList = new BindingList<Person>();
            this.groupsListBox.DataSource = this.personList;
            this.rng = new Random();
            this.layout1RadioButton.Checked = true;
            this.EditListButton_Click(this, new EventArgs());
        }

        /// <summary>
        /// Shuffles the people list.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Additional event arguments.</param>
        private void ShuffleButton_Click(object sender, EventArgs e)
        {
            this.personList.Shuffle();
        }

        /// <summary>
        /// Picks one item of the listBox randomly.
        /// </summary>
        /// <remarks>
        /// Jumps the selected item in the listBox 20 times slowing down gradually.
        /// </remarks>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Additional event arguments.</param>
        private void PickOneButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(20 * i);
                this.groupsListBox.SetSelected(this.rng.Next(this.personList.Count), true);
            }
        }

        /// <summary>
        /// Loads an example list of people into the listBox.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Additional event arguments.</param>
        private void LoadExampleListButton_Click(object sender, EventArgs e)
        {
            this.personList.Clear();
            this.personList.Add(new Student("Mosby, Ted Evelyn (6978639)"));
            this.personList.Add(new Student("Eriksen, Marshall (6961326)"));
            this.personList.Add(new Student("Scherbatsky, Robin (6375003)"));
            this.personList.Add(new Student("Stinson, Barney (6236471)"));
            this.personList.Add(new Student("Aldrin, Lily (0756495)"));
            this.personList.Add(new Student("McConnell, Tracy (7936213)"));
            this.personList.Add(new Student("Singh, Ranjit (3874496)"));
            this.personList.Add(new Student("Rivers, Sandy (4193406)"));
            this.personList.Add(new Student("MacLaren, Carl (5965287)"));
            this.personList.Add(new Student("Eriksen, Marvin (1085283)"));
            this.personList.Shuffle();
        }

        /// <summary>
        /// Splits the people list into groups of a specified size and displays them.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Additional event arguments.</param>
        private void SplitIntoGroupsOfButton_Click(object sender, EventArgs e)
        {
            int maxGroupSize = GroupsFormHelpers.GetNumberFrom(this.groupSizeBox);
            if (maxGroupSize == 0)
            {
                return;
            }

            this.personList.Shuffle();

            int numberOfGroups = this.personList.Count / maxGroupSize;
            if (this.personList.Count % maxGroupSize != 0)
            {
                numberOfGroups++;
            }

            Person[,] groups = GroupsFormHelpers.SplitIntoGroups(this.personList, numberOfGroups, maxGroupSize);

            DisplayGroupsForm displayGroupsForm = new DisplayGroupsForm(groups);
            displayGroupsForm.ShowDialog();
        }

        /// <summary>
        /// Executes the SplitIntoGroupsOfButton_Click method on clicking "Enter" in the groupSizeBox.
        /// </summary>
        /// <param name="sender">TextBox that was typed in.</param>
        /// <param name="e">Additional event arguments.</param>
        private void GroupSizeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SplitIntoGroupsOfButton_Click(this, new EventArgs());
            }
        }

        /// <summary>
        /// Splits the people list into specified number of groups and displays them.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Additional event arguments.</param>
        private void SplitIntoGroupsButton_Click(object sender, EventArgs e)
        {
            int numberOfGroups = GroupsFormHelpers.GetNumberFrom(this.numberOfGroupsBox);
            if (numberOfGroups == 0)
            {
                return;
            }

            this.personList.Shuffle();

            int maxGroupSize = this.personList.Count / numberOfGroups;
            if (this.personList.Count % numberOfGroups != 0)
            {
                maxGroupSize++;
            }

            Person[,] groups = GroupsFormHelpers.SplitIntoGroups(this.personList, numberOfGroups, maxGroupSize);

            DisplayGroupsForm displayGroupsForm = new DisplayGroupsForm(groups);
            displayGroupsForm.ShowDialog();
        }

        /// <summary>
        /// Executes the SplitIntoGroupsButton_Click method on clicking "Enter" in the numberOfGroupsBox.
        /// </summary>
        /// <param name="sender">TextBox that was typed in.</param>
        /// <param name="e">Additional event arguments.</param>
        private void NumberOfGroupsBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SplitIntoGroupsButton_Click(this, new EventArgs());
            }
        }

        /// <summary>
        /// Opens the ImportForm to modify the personList.
        /// </summary>
        /// <remarks>
        /// Opened at the start for initial import.
        /// </remarks>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Additional event arguments.</param>
        private void EditListButton_Click(object sender, EventArgs e)
        {
            ImportForm importForm = new ImportForm(this.personList);
            /* Attention, there is an error in the book about this (p. 634)!
             * Modeless forms are displayed with the Show() function.
             * However, we want a modal form:
             */
            importForm.ShowDialog();
        }

        /// <summary>
        /// Refreshes the data in the groupListBox.
        /// </summary>
        private void RefreshGroupsListBox()
        {
            this.groupsListBox.DataSource = null;
            this.groupsListBox.DataSource = this.personList;
        }

        /// <summary>
        /// When the radioButton is active, it sets the default display.
        /// </summary>
        /// <param name="sender">The Radio button is clicked.</param>
        /// <param name="e">Additional Event arguments.</param>
        private void layout1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Student.DisplayNumber = 1;
            this.RefreshGroupsListBox();
        }

        /// <summary>
        /// When the radioButton is active, it sets the display to setting 2.
        /// </summary>
        /// <param name="sender">The Radio button is clicked.</param>
        /// <param name="e">Additional Event arguments.</param>
        private void layout2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Student.DisplayNumber = 2;
            this.RefreshGroupsListBox();
        }

        /// <summary>
        /// When the radioButton is active, it sets the display to setting 3.
        /// </summary>
        /// <param name="sender">The Radio button is clicked.</param>
        /// <param name="e">Additional Event arguments.</param>
        private void layout3RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Student.DisplayNumber = 3;
            this.RefreshGroupsListBox();
        }
    }
}