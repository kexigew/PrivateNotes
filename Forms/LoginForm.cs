﻿using PrivateNotes.Controllers;
using PrivateNotes.Forms;
using PrivateNotes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrivateNotes {
	public partial class Login : Form {
		public Login() {
			InitializeComponent();
		}
		// yeet test
        private void Form1_Load(object sender, EventArgs e) {
		}

        private void loginButton_Click(object sender, EventArgs e) {
			if (usernameInput.Text.Length >= 1) {

				LoginCredentials lg = new LoginCredentials(usernameInput.Text, passwordInput.Text);
				if (!LoginController.Login(lg, this)) {
					// password invalid
					incorrect.Visible = true;
				}
			}
            else {
				incorrect.Visible = true;
            }
		}

        private void createAccoutButton_Click(object sender, EventArgs e) {
			this.Hide();
			var rf = new RegisterForm();
			rf.ShowDialog();
			this.Close();
		}
		
	}
}
