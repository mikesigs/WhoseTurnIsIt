namespace WhoseTurnIsIt

open Newtonsoft.Json.Serialization
module Model =

    type User = {
        userId: string;
        lastSeen: System.DateTimeOffset;
        locale: string;
    }

    type OriginalRequestData = {
        user: User;
    }

    type OriginalRequest = {
        data: OriginalRequestData;
    }

    type WhoPickedWhatParameters = {
        whoPicked: string;
        thingType: string;
        thingDescription: string;
    }
    
    type WhoPickedWhatRequestResult = {
        parameters: WhoPickedWhatParameters;
    }

    type WhoPickedWhatRequest = {
        result: WhoPickedWhatRequestResult;
        originalRequest: OriginalRequest;
    }

    type GoogleActionResponse = {
        speech: string;
        displayText: string;
        source: string;
    }