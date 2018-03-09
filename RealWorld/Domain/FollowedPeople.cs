﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Domain
{
    public class FollowedPeople
    {
        public int ObserverId { get; set; }
        public Person Observer { get; set; }

        public int TargetId { get; set; }
        public Person Target { get; set; }
    }
}
