﻿namespace IP_8IEN.BL.Domain.Data
{
    public class SubjectMessage
    {
        public int SubjectMessageId { get; private set; }
        public Message Message { get; private set; }
        public Onderwerp Onderwerp { get; private set; }
    }
}