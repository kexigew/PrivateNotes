﻿using PrivateNotes.Forms;
using PrivateNotes.Models;
using PrivateNotes.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PrivateNotes.Controllers {
	class LoginController {

		private static LoginCredentials loggedUser = new LoginCredentials();

		public static bool Login(LoginCredentials loginCredentials, Form form) {

			if (loginCredentials.IsValid()) {
				// start notes page
				loggedUser.username = loginCredentials.username;
				loggedUser.password = loginCredentials.password;

				DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + loginCredentials.Username);
				FileInfo[] Files = d.GetFiles("*.aes"); 
				//Getting Text files
				foreach (FileInfo file in Files) {
					string output = file.Name.Split('.')[0];
					Encryption.FileDecrypt(
						Directory.GetCurrentDirectory() + "\\" + loginCredentials.Username + "\\" + file.Name,
						Directory.GetCurrentDirectory() + "\\" + loginCredentials.Username + "\\" + output + ".txt",
						loginCredentials.password
					);
				}

                form.Hide();
				var nt = new NotesForm();
				nt.ShowDialog();
				form.Close();
			}
			return false;
		}

		public static Boolean Register(LoginCredentials loginCredentials) {
			// get the bytes to store from the credentials
			byte[] bytes = loginCredentials.GetBytes();

			// base64 of the username
			string username = loginCredentials.Username;

			// read through the bytes to see if username exists 
			byte[] rawBytes = ReadStoredBytes();

			if (rawBytes is null)
				return false;

			for (int i = 0; i < rawBytes.Length / 288; i++) {
				int lowerIndex = 288 * i;
				byte[] user = rawBytes[lowerIndex..(lowerIndex + 288)];
				if(username.Equals(Convert.ToBase64String(user[0..32])))
					return false;
			}

			// write the bytes to the file
			using (var stream = new FileStream("credentials.dat", FileMode.Append)) {
				stream.Write(bytes, 0, bytes.Length);
			}

			Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + loginCredentials.Username);

			return true;
		}

		public static byte[] GetHash(string username) {

			// all bytes stored in file
			byte[] rawBytes = ReadStoredBytes();

			if (rawBytes is null)
				return null;

			// dictionary of users and hash
			Dictionary<string, byte[]> credentials = new Dictionary<string, byte[]>();

			// for each user add it to the dictionary
			for (int i = 0; i < rawBytes.Length / 288; i++) {
				int lowerIndex = 288 * i;
				byte[] user = rawBytes[lowerIndex..(lowerIndex + 288)];
				credentials.Add(Convert.ToBase64String(user[0..32]), user[32..288]);
			}

			// return specific user
			if (credentials.ContainsKey(username))
				return credentials[username];
			else
				return null;

		}

		public static byte[] ReadStoredBytes() {
			byte[] rawBytes = null;

			try {
				rawBytes = File.ReadAllBytes("credentials.dat");
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}

			return rawBytes;
		}

		public static LoginCredentials GetLoginCredentials() {
			return loggedUser;
        }

	}
}
