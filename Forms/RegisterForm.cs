﻿using PrivateNotes.Controllers;
using PrivateNotes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PrivateNotes.Forms {
    public partial class RegisterForm : Form {
        public RegisterForm() {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, EventArgs e) {
            if (usernameInputRegister.TextLength >= 1) {
                if (passwordInput.Text.Equals(confirmPasswordInput.Text)) {
                    LoginCredentials credentials = new LoginCredentials(usernameInputRegister.Text, passwordInput.Text);
                    if (LoginController.Register(credentials)) {
                        registerButton.Enabled = false;
                        this.Hide();
                        var login = new Login();
                        login.ShowDialog();
                        this.Close();
                    }
                    else {
                        // show user that someone already has an account named that way
                        incorrect2.Show();
                    }
                }
                else
                    incorrect.Show();
            }
            else {
                incorrectUsername2.Visible = true;
            }
        }

    }
}
