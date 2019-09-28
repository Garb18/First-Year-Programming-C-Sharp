using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;      // Required to use XNA features.
using XNAMachinationisRatio;        // Required to use the XNA Machinationis Ratio Engine.
using XNAMachinationisRatio.AI;     // Required to use the XNA Machinationis Ratio general AI features


namespace FishORama
{
    class PiranhaToken : X2DToken
    {
        #region Data members

        // This token needs to interact with the aquarium to swim in it (it needs information
        // regarding the aquarium's boundaries). Hence, it needs a "link" to the aquarium,
        // which is why it stores in an instance variable a reference to its aquarium.

        private AquariumToken mAquarium;    // Reference to the aquarium in which the creature lives.
        private PiranhaMind mMind;          // Explicit reference to the mind the token is using to enact its behaviors.
        private int piranhaNumber;
        private int teamNumber;

        #endregion

        #region Properties

        /// <summary>
        /// Get aquarium in which the creature lives.
        /// </summary>
        public AquariumToken Aquarium
        {
            get { return mAquarium; }
        }

        //Passes the number of the piranha out of the class
        public int PiranhaNumber
        {
            get { return piranhaNumber; }
        }

        //Passes the team number out of the class
        public int TeamNumber
        {
            get { return teamNumber; }
        }
        
        #endregion

        #region Constructors

        /// Constructor for the orange fish.
        /// Uses base class to initialize the token name, and adds code to
        /// initialize custom members.
        /// <param name="pTokenName">Name of the token.</param>
        /// <param name="pAquarium">Reference to the aquarium in which the token lives.</param>
        /// 

        public PiranhaToken(String pTokenName, AquariumToken pAquarium, int pNumber, int tNumber)
            : base(pTokenName)
        {
            mAquarium = pAquarium;                  // Store reference to aquarium in which the creature is living.
            mMind.Aquarium = mAquarium;             // Provide to the mind a reference to the aquarium, required to swim appropriately.

            piranhaNumber = pNumber;                // Parameter Assignment
            teamNumber = tNumber;                   // Parameter Assignment

            mMind.PiranhaNumber = piranhaNumber;    // Assigns values passed out to the mind
            mMind.TeamNumber = teamNumber;          // Assigns values passed out to the mind
        }

        #endregion

        #region Methods

        /// <summary>
        /// Setup default properties of the token.
        /// </summary>
        protected override void DefaultProperties()
        {

            // Specify which image should be associated to this token, assigning
            // the name of the graphic asset to be used ("OrangeFishVisuals" in this case)
            // to the property 'GraphicProperties.AssetID' of the token.

            //Assigns green pirahna visual
            this.GraphicProperties.AssetID = "PiranhaVisuals1";

            //if (teamNumber == 1)
            //{
            //    this.GraphicProperties.AssetID = "PiranhaVisuals1";
            //}
            //else if (teamNumber == 2)
            //{
            //    this.GraphicProperties.AssetID = "PiranhaVisuals2";
            //}

            // Specify mass of the fish. This can be used by
            // physics-based behaviors (work in progress, not functional yet).
            this.PhysicsProperties.Mass = 3;

            
            PiranhaMind myMind = new PiranhaMind(this);   // Create mind, implicitly associating it to the token.


            mMind = myMind;                 // Store explicit reference to mind being used.
            mMind.Aquarium = mAquarium;     // Provide to mind explicit reference to Aquarium.
        }

        #endregion

    }
}