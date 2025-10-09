using System.Xml.Linq;

private string AccountNo()
{
    string startWith = "2280";
    Random random = new Random();
    string gen = random.Next(0, 888888).ToString("D5");
    string AccountNo = startWith + gen;
    return AccountNo;
}

private void btnWithdraw_Click(object sender, EventArgs e)
{
    int balance = 0;
    SqlConnection con = new SqlConnection("(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-BankingWinAppForms-651306ba-5b89-4ba9-be3e-0b6920d6338a;Integrated Security=True;TrustServerCertificate=True");
    con.Open();
    string readQuery = "SELECT * FROM Account WHERE AccountNo = @Acc";
    SqlCommand cmd = new SqlCommand(readQuery, con);
    cmd.Parameters.AddWithValue("@Acc", txtAcc.Text);
    int Count = Convert.ToInt32(cmd.ExecuteScalar());
    if (Count > 0) ;
    {
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            balance = Convert.ToInt32(reader["balance"]);
        }
        reader.Close();
        if (balance > 0 && balance > Convert.ToInt32(txtBalance.Text))
        {
            int newBalance = balance - int.Parse(txtBalance.Text);
            string updateQuery = "Update Account SET balance = @balance, Withdraw = @with WHERE AccountNo = @AccNo";
            SqlCommand updateCmd = new SqlCommand(updateQuery, con);
            updateCmd.Parameters.AddWithValue("@balance", newBalance);
            updateCmd.Parameters.AddWithValue("@with", txtBalance.Text);
            updateCmd.Parameters.AddWithValue("@AccNo", txtAcc.Text);
            updateCmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Remaining Balance" + newBalance);
        }
        else
        {
            MessageBox.Show("Insufficient Balance", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

private void btnShow_Click(object sender, EventArgs e)
{
    SqlConnection con = new SqlConnection("(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-BankingWinAppForms-651306ba-5b89-4ba9-be3e-0b6920d6338a;Integrated Security=True;TrustServerCertificate=True");
    con.Open();
    string selectQuery = "SELECT * FROM Account WHERE AccountNo = @Acc";
    SqlCommand cmd = new SqlCommand(selectQuery, con);
    cmd.Parameters.AddWithValue("@Acc", txtAcc.Text);
    SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        txtName.Text = reader["name"].ToString();
        txtAdd.Text = reader["address"].ToString();
        txtBalance.Text = reader["balance"].ToString();
    }
    reader.Close();
    con.Close();
}

private void btnClear_Click(object sender, EventArgs e)
{
    SqlConnection con = new SqlConnection("(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-BankingWinAppForms-651306ba-5b89-4ba9-be3e-0b6920d6338a;Integrated Security=True;TrustServerCertificate=True");
    con.Open();
    string clearQuery = "DELETE FROM Account WHERE AccountNo = @Acc";
    SqlCommand cmd = new SqlCommand(clearQuery, con);
    cmd.Parameters.AddWithValue("@Acc", txtAcc.Text);
    if (MessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ;

    else
    {
        return;
    }
    con.Close();
    int Count = cmd.ExecuteNonQuery();
    dtDate.Enabled = false;
}
