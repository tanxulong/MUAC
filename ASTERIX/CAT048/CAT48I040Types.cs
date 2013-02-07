﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsterixDisplayAnalyser
{
    class CAT48I040Types
    {
        public class CAT48I040MeasuredPosInPolarCoordinates
        {
            public double Measured_Distance;
            public double Measured_Azimuth;
            public GeoCordSystemDegMinSecUtilities.LatLongClass LatLong = new GeoCordSystemDegMinSecUtilities.LatLongClass();
        }
    }
}
