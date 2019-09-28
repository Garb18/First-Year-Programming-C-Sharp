using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;              // Required to use XNA features.
using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio Engine general features.
using XNAMachinationisRatio.AI;             // Required to use the XNA Machinationis Ratio general AI features.


namespace FishORama
{
    class AquariumMind : AIPlayer
    {
        #region Data Members

        // This mind needs to interact with the token which it possesses, 
        // since it needs to know where are the aquarium's boundaries.
        // Hence, the mind needs a "link" to the aquarium, which is why it stores in
        // an instance variable a reference to its aquarium.
        private AquariumToken mAquarium = null;         // Reference to the aquarium in which the creature lives.

        static Random numberGenerator = new Random();
        int chickenLegPlacement;


        #endregion

        #region Properties

        /// <summary>
        /// Set Aquarium in which the mind's behavior should be enacted.
        /// </summary>
        public AquariumToken Aquarium
        {
            set { mAquarium = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="pToken">Token to be associated with the mind.</param>
        public AquariumMind(X2DToken pToken)
        {
            /* LEARNING PILL: associating a mind with a token
             * In order for a mind to control a token, it must be associated with the token.
             * This is done when the mind is constructed, using the method Possess inherited
             * from class AIPlayer.
             */
            this.Possess(pToken);       // Possess token.           
        }

        #endregion

        #region Methods

        /// <summary>
        /// AI Update method.
        /// </summary>
        /// <param name="pGameTime">Game time</param>
        public override void Update(ref GameTime pGameTime)
        {
            CheckChickenLeg();
        }

        /// <summary>
        /// Check if the user clicks left mouse button. If the user clicked in the Aquarium,
        /// and if the leg is not already in the aquarium, place a new leg at click
        /// position.
        /// </summary>
        public void CheckChickenLeg()
        {
            if (!PiranhaMind.win)
            {
                //30 game ticks a second, so should get a chicken leg spawn ~4 seconds, but depends on chicken leg != null
                //Can change this number to speed up/slow down the pace of the game

                chickenLegPlacement = numberGenerator.Next(0, 121);

                if (chickenLegPlacement == 1 && mAquarium.ChickenLeg == null)
                {
                    // Uses Vector 2 for use with scence's camera, repurposed from old CheckLeftClick behaviour method
                    Vector2 chickLegPos = new Vector2(400, numberGenerator.Next(-50, 351));
                    Vector3 legPos = this.mAquarium.Kernel.Camera.CameraToWorld(chickLegPos);

                    //Reused CheckLeftClick behaviour code, sets Z axis to 3 to ensure it's infront of the scene
                    if ((Math.Abs(legPos.X) < mAquarium.Width / 2) && (Math.Abs(legPos.Y) < mAquarium.Height / 2))
                    {
                        mAquarium.ChickenLeg = new ChickenLegToken("ChickenLeg");
                        legPos.Z = 3;

                        this.mAquarium.Kernel.Scene.Place(mAquarium.ChickenLeg, legPos);
                    }
                }
            }
        }
        #endregion
    }

}
