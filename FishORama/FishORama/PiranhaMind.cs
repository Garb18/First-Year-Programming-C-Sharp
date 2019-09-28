using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;              // Required to use XNA features.
using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio Engine general features.
using XNAMachinationisRatio.AI;             // Required to use the XNA Machinationis Ratio general AI features.
using System.Diagnostics;
using System.Threading;

namespace FishORama
{
    class PiranhaMind : AIPlayer
    {
        #region Data Members

        // This mind needs to interact with the token which it possesses, 
        // since it needs to know where are the aquarium's boundaries.
        // Hence, the mind needs a "link" to the aquarium, which is why it stores in
        // an instance variable a reference to its aquarium.

        private AquariumToken mAquarium;                        // Reference to the aquarium in which the creature lives.

        private static Random numberGenerator = new Random();   // Static random number generator to prevent the same number issue that can occure from multiple

        private int piranhaNumber;                              // Which number the fish is between 1 - 3
        private int teamNumber;                                 // Which team the fish is on
        
        private float mFacingDirectionX = 1;                    // Direction the fish is facing (1: right; -1: left).

        private readonly float mSpeedX = 5;                     // Speed in which the Piranha swims

        private Vector3 mFeedingPosition;                       // Vector storing the chicken leg position
        private Vector3 startingPosition;                       // Starting position of the Piranha, used for relative calculation for movement towards chicken leg
       
        private bool checkLegX = false;                         // X positional check for the chicken leg in relation to the piranha 
        private bool checkLegY = false;                         // Y positional check for the chicken leg in relation to the piranha 

        private float radius;                                   // Radius of the circle in which the Piranha swims
        private float speed;                                    // Speed of which the Pirahna swims in a circle

        private static int numInPlay;                           // Which piranha number is playing in the round
        private static bool active = false;                     // Says if the fish is participating in the round
        private static int team_1_Score;                        // The score for team one
        private static int team_2_Score;                        // The score for team two

        private bool Initilised = true;                         // One time run Initilisation bool
        public static bool win = false;                         // Bool triggered upon one team winning the game
        private static bool Victory_Screetch = true;            // Bool used for declaring winning game in console log
        #endregion

        #region Properties

        /// <summary>
        /// Set Aquarium in which the mind's behavior should be enacted.
        /// </summary>
        public AquariumToken Aquarium
        {
            set { mAquarium = value; }
        }

        //Catches and sets the piranha number from the token class
        public int PiranhaNumber
        {
            set { piranhaNumber = value; }
        }

        //Catches and sets the team number from the token class
        public int TeamNumber
        {
            set { teamNumber = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="pToken">Token to be associated with the mind.</param>

        public PiranhaMind(X2DToken pToken)
        {
            this.Possess(pToken);
        }

        #endregion

        #region Methods

        private void Initilisation()//One time run initilisation of the Piranha
        {
            //Sets vector position of the Piranha
            startingPosition = this.PossessedToken.Position;
            //Ensures the Piranha's are facing the right way based on which team they are a part of

            //Team1 faces right
            if (teamNumber == 1)
            {
                mFacingDirectionX = -1;
            }
            //Team2 faces left
            else if (teamNumber == 2)
            {
                mFacingDirectionX = 1;
            }

            //Randomly selects the first fish to play
            numInPlay = numberGenerator.Next(1, 4);

            //Disables the one time run initlisation
            Initilised = false;
        }

        private void IdleSwim()//Circle swimming behaviour whilst piranha not in play
        {
            // Radius of the circle in which the Piranha swims
            radius = 10;
            //Speed in which they swim inside a circle
            speed += 0.08f;

            //Standard tokenposition converstion to a vector
            Vector3 tokenPosition = this.PossessedToken.Position;
            //Vector manipulation using cos/sin circle mathematics
            tokenPosition.Y = (float)Math.Cos(speed) * radius + startingPosition.Y;
            tokenPosition.X = (float)Math.Sin(speed) * radius + startingPosition.X;
            //Re-assigns the vector value to the Piranha
            this.PossessedToken.Position = tokenPosition;

            //Determines the orientation of the piranha
            this.PossessedToken.Orientation = new Vector3(mFacingDirectionX,
                                                        this.PossessedToken.Orientation.Y,
                                                        this.PossessedToken.Orientation.Z);
        }

        private void RacingBehaviour()//Vector maths for piranha swimming towards chicken leg
        {
            //Standard tokenposition converstion to a vector
            Vector3 tokenPosition = this.PossessedToken.Position;

            //Passes through the position of the chicken leg to the Piranha
            mFeedingPosition = tokenPosition - mAquarium.ChickenLeg.Position;
            //Vector normalisation to prevent piranha swimming off of the stage in one game tick
            mFeedingPosition.Normalize();
            //Movement towards the chicken leg
            tokenPosition = tokenPosition - mFeedingPosition * mSpeedX;
            //Reassigns values after calculations have been completed
            this.PossessedToken.Position = tokenPosition;

            //Two bools check for the X and Y axis to check piranha position relative to the ChickenLeg
            //X bool positional check
            if (tokenPosition.X - mAquarium.ChickenLeg.Position.X <= 3 && tokenPosition.X - mAquarium.ChickenLeg.Position.X >= -3)
            {
                checkLegX = true;
            }

            //Y bool positional check
            if (tokenPosition.Y - mAquarium.ChickenLeg.Position.Y <= 3 && tokenPosition.Y - mAquarium.ChickenLeg.Position.Y >= -3)
            {
                checkLegY = true;
            }

            //If both bools return true - then leg is set to null, and bools are reset
            if (checkLegX && checkLegY == true)
            {
                //Adds score to the winning team of that round
                if (teamNumber == 1)
                {
                    //Adds one to team ones score
                    team_1_Score += 1;
                }
                else if (teamNumber == 2)
                {
                    //Adds one to team twos score
                    team_2_Score += 1;
                }

                //Reset code
                //Removes Chicken Leg from scene
                mAquarium.RemoveChickenLeg();
                //Resets bool checks for position of pirahna in relation to chicken leg
                checkLegX = false;
                checkLegY = false;
                // Sets the fish to inactive
                active = false;
                //Selects the next Piranha that plays
                PiranhaPlayer();

                //Prints out the score of each team
                Console.WriteLine("Team 1 score = {0}", team_1_Score);
                Console.WriteLine("Team 2 score = {0}", team_2_Score);
            }
        }

        private void CelebrationSwimBehaviour()//Standard horizontal swim behaviour, reused from basic OrangeFish in Milestone 1
        {
            //Vector assignment
            Vector3 tokenPosition = this.PossessedToken.Position;
            //Vector Manipulation
            tokenPosition.X = tokenPosition.X - (mSpeedX * mFacingDirectionX);
            //Vector value re-assignment
            this.PossessedToken.Position = tokenPosition;
            //Standard vector orientation maths
            this.PossessedToken.Orientation = new Vector3(mFacingDirectionX,
                                                        this.PossessedToken.Orientation.Y,
                                                        this.PossessedToken.Orientation.Z);

            //Checks upon reaching the edge of the horizontal boundary of the tank
            //Vertical edge check not needed as fish only swim left or right
            if (this.mAquarium.ReachedHorizontalBoundary(this.PossessedToken))
            {
                //Reverses the fish
                mFacingDirectionX = -mFacingDirectionX;
            }
        }

        private void PiranhaPlayer()//Chooses next piranha in play
        {
            // Triggers upon the chicken leg being reached
            if (!active)
            {
                //Randomly chooses the next Piranha which will be played
                numInPlay = numberGenerator.Next(1, 4);
            }
        }

        private void GameWin()//Game winning triggers such as winning decleration
        {
            //If Team 1 wins
            if (teamNumber == 1 && team_1_Score ==5)
            {
                win = true;
                //Swim around the tank
                CelebrationSwimBehaviour();
                //Declare winner
                if (Victory_Screetch)
                {
                    Console.WriteLine("Team 1 wins!");
                    Victory_Screetch = false;
                }
            }
            //If Team 2 wins
            else if (teamNumber == 2 && team_2_Score == 5)
            {
                win = true;
                //Swim around the tank
                CelebrationSwimBehaviour();
                //Declare winner
                if (Victory_Screetch)
                {
                    Console.WriteLine("Team 2 wins!");
                    Victory_Screetch = false;
                }
            }
            else
            {
                IdleSwim();
            }
        }

        private void PiranhaBehaviour()//Meta method consolidating all behaviours and when they trigger
        {
            //One time run initilisation
            while (Initilised)
            {
                Initilisation();
            }

            if (!win)
            {
                //Sets a pirahna to active upon chicken leg being placed in scence
                if (mAquarium.ChickenLeg != null && piranhaNumber == numInPlay)
                {
                    active = true;
                    RacingBehaviour();
                }
                else
                {
                    //Pirahnas idle waiting for Chicken Leg
                    IdleSwim();
                }
            }

            //Winning score trigger
            if (team_1_Score == 5 || team_2_Score == 5)
            {
                GameWin();
            }
        }

        /// <summary>
        /// AI Update method.
        /// </summary>
        /// <param name="pGameTime">Game time</param>
        /// 
        public override void Update(ref GameTime pGameTime)
        {
            PiranhaBehaviour();   
        }

        #endregion
    }
}