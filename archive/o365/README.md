# Building an IVR with Microsoft Bot Framework 


```
**Private preview program is currently full**. We are not adding additional customers at this time. 
```

Telphony channel in Microsoft Bot Framework is the Microsoft technology for enabling PSTN calling capabilites in a Bot. Telephony channel allows you to build an IVR by leveraging Office 365 phone numbers, along with the full power of Microsoft Azure Bot Framework and Microsoft Speech Services.

 ![](images/telephonychannel.png)

Please note:  This is a Beta (preview) version of software, and as with any preview software, there may be initial risks and limitations you run into, such as a need to integrate with your existing IVR, etc.  We are actively working on and supporting this product and are here to help you in case you run into any issues.  Reach us at ms-ivr-preview@microsoft.com.

**General Availability**:  due to the shifting impact of COVID-19, we have not set the GA date yet and are instead working with private preview customers directly on their IVR's.

## Requirements

**NOTE**: Private preview program is currently full. We are not accepting new applications at this time.

```
COVID-19 UPDATE:  We are receiving a growing number of requests for private preview for COVID-19 call 
center bots.  Since the product is currently in preview, we are not approving COVID-19 bots at this time.  
We recommend https://www.qnamaker.ai/ bots for web and messaging channels.
```

* **IVR Private Preview Approval (Currently FULL - not accepting new requests)** - To get started, your Tenant/Organization needs to be approved for a Private Preview of the Microsoft Intelligent Call Center / IVR project.  Good candidates for the preview are existing Bot Framework customers with existing production bots (ideally those that have Office 365 E5 subscriptions as well) and a dedicated Microsoft account manager (including MSC/CSA). Please have your Microsoft account manager send an e-mail to ms-ivr-preview@microsoft.com from their @microsoft.com e-mail address with following information:
  * Tenant/Organization name
  * Azure account e-mail that should be whitelisted
  * Description of the bot
  * Expected call volumes while in public preview.
  
We are not approving general "evaluation" requests or PoCs. Please only submit requests if you have actual target users who would be able to dial your IVR and use it if you are whitelisted.

  Once approved for privare preview, the Azure account provided will see Telephony channel in their bot settings. 
* **Office 365 License** - A minimum of Office 365 E3 + calling plan or an E5 plan is required 
* **Azure Subscription** - You will need a valid Azure subscription.

# Enabling IVR 

After getting approved into the private preview, overall setup should take roughly an hour to enable a basic IVR bot callable using a phone number.

The following are the high-level steps needed you to enable IVR support in your bot:

* [Optional: create a new Office 365 account trial](CreateOfficeTrial.md)
* [Step 1: Provision a new phone number for your bot in Office 365](AcquirePhoneNumber.md)
* [Step 2: Create a new Azure Web App Bot](CreateBot.md)
* [Step 3: Enable your bot to speak and understand voice](CreateSpeechResource.md)
* [Step 4: Enable Telephony Channel](EnableTelephony.md)
* [Step 5: Process speech inside of the bot](ProcessSpeechInBotCode.md)
* [Step 6: Transfer call to an agent](TransferCallOut.md)
* [Step 7: Terminating the ongoing call](TerminateCall.md)
* [Troubleshooting](TroubleshootingTelephonyBot.md)

Once setup, you should be able to simply dial the acquired phone number using any PSTN or mobile phone (subjected to cellular plan on the source phone).
