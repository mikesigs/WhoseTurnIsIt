namespace WhoseTurnIsIt

module DialogFlow =

    type DialogFlowRequest = {
        contexts: string seq;
        lang: string;
        query: string
        sessionId: string
        timezone: string
    }

    type DialogFlowResponse<'TData, 'TContextOut, 'TFollowUpEvent> = {
        speech: string;
        displayText: string;
        data: 'TData;
        contextOut: 'TContextOut seq;
        source: string;
        followupEvent: 'TFollowUpEvent
    }