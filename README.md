# SQS Manager - managing queue's better!

Simple WPF application to make it a little easier to do stuff with our queue's.

*What we currently have in this version:*
- redriving and downloading messages (automatically)
- purging messages from a queue (optional)

*requirements:*
- Python
- Okta-AWSCLI

*select profile from powershell using Okta-AWSCLI*

```
pip install okta-awscli
okta-awscli --okta-profile pmgroup-prod 
Enter password:
Multi-factor Authentication required for application.
Enter MFA verification code:
```

*Double check if .aws folder contains the following:*
- config file
- credentials file

*Double check if your user folder contains the following:*
- .okta-aws file -> This file should look similar to the config file in .aws folder

Run the .exe and have fun! :)