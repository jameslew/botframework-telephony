﻿﻿﻿# Serial Numbers Post Processing 

Prompting end users for serial numbers via speech enabled bots has proven to be quite challenging for the following reasons:
- Speech-to-text system returns words representation of the alphanumeric characters.
- Certain letters are translated as numbers or other letters, ie 'A' turns into '8'.
- Certain combination of letters are translated as words, ie "KT" turns into "Katie".
- No support for using military code to disambiguate alphabets, ie "A as in Apple".
- Speech-to-text translation errors are different in different languages.

Speech server team is aware of these issues and are planning a set of features to address the issues, 
among them are custom display post processing and prebuilt types. However these features will not be available until later this year.

SerialNumberInput and AlphaNumericSequencePostProcessor are created to address most of the speech-to-text translation issues from 
specifying serial numbers.  

## SerialNumberInput
SerialNumberInput is a Dialog class for prompting serial number inputs.  
It aggregates input until input after post processing matches with the RegexPattern and then stores the result in an output property.

Notable Parameters:
- RegexPattern: serial number pattern
- Property: property to assign final serial number to
- Prompt: text to prompt user for serial number
- ConfirmationPrompt: text to prompt user to confirm ambiguous choices

Example usage:
In the following example, a serial number is valid when it has alphanumeric characters with length of 5.

```
"actions": [
        {
          "$kind": "Microsoft.Telephony.SerialNumberInput",
          "property": "conversation.DTMFResult",
          "prompt": "Enter serial number number.",
          "continuePrompt": "Please continue with next letter or digit.",
          "confirmationPrompt": "Say or type 1 for {0} or 2 for {1}",
          "regexPattern": "([0-9a-zA-Z]{5})",
          "allowInterruptions": "=coalesce(settings.allowInterruptions, true)",
          "interruptionMask": "^[\\d]+$",
          "alwaysPrompt": "=coalesce(settings.alwaysPrompt, false)"
        },
        {
          "$kind": "Microsoft.SendActivity",
          "activity": "${coalesce(conversation.DTMFResult, 'empty')}"
        }
      ]
```



## AlphaNumericSequencePostProcessor

AlphaNumericSequencePostProcessor class is initialized with a list of text groups or a simplified regex expression which represents the text groups.
A text group represents a fixed length set of letters or numbers.  The more specific the definition of each text group, the more accurate the result of the post processing. 
In a group of alphabet only set, '8' could be auto corrected to 'A'.  
If one or more results are returned, there are ambiguous choices and user should be prompted to confirm the right sequence.

AlphaNumericSequencePostProcessor class supports inputting letters with military code, ie "A as in Alpha", "B as in Beta", etc.  User could use any word after the "as in" phrase.

Sample usage:

    var groups = new List<TextGroup>();
    // The first char can only be letter
    var g1 = new TextGroup
    {
    AcceptsDigits = false,
    AcceptsAlphabet = true,
    LengthInChars = 1,
    };
    groups.Add(g1);
        
    // The next three characters could be alphanumeric
    var g2 = new TextGroup
    {
        AcceptsDigits = true,
        AcceptsAlphabet = true,
        LengthInChars = 3,
    };
    groups.Add(g1);
    
    // Test the pattern with valid input with military code
    var input = "ABC, as in Charlie Z as in Zeta.";
    var pattern = new AlphaNumericSequencePostProcessor(groups.AsReadOnly());
    var result = pattern.Inference(input);
    
    // result[0] should be "ABCZ"
    
    // Test the pattern with valid input common STT translation error
    input = "8 B D as in Dog T as in Tom.";
    result = pattern.Inference(input);
    
    // result[0] should be "ABDT"
    
     // Test the pattern with invalid input
    input = "4XYZ";
    result = pattern.Inference(input);
    
    // invalid input, result.Length is 0


Currently, post processing for English language is supported.  We are working to add support for Spanish and French.



