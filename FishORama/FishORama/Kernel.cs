using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio general features.
using XNAMachinationisRatio.Resource;       // Required to use the MonoGame Machinationis Ratio resource management features.

namespace FishORama
{
    /// <summary>
    /// Kernel (orchestrator class) for this application.
    /// </summary>
    public class Kernel : XNAGame
    {

        #region Data Members

        
        I2DScene mScene = null;                         // Reference to the FishORama scene, set to null before its initialization.
                                                        // Creation and initialization is performed in the LoadContent method.
        
       
        I2DCamera mCamera = null;                       // Reference to the FishORama camera, set to null before its initialization.
                                                        // Creation and initialization is performed in the LoadContent method.

        Vector3 piranhaPos;                             //Position vector for the Piranha spawns
        PiranhaToken[] piranha = new PiranhaToken[3];   //Stores piranha objects inside an array, used inside a nested for loop

        int x = -300;                                   //Hard coded X position of the Piranha - Position is manipulated within the nested for loop
        int y = -150;                                   //Hard coded Y position of the Piranha - Position is manipulated within the nested for loop

        #endregion

        #region Properties

        /// <summary>
        /// Get simulation scene.
        /// </summary>
        public I2DScene Scene
        {
            get { return mScene; }
        }

        /// <summary>
        /// Get simulation camera.
        /// </summary>
        public I2DCamera Camera
        {
            get { return mCamera; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Kernel(): base("FishO'Rama")
        {
            this.IsMouseVisible = true;     // Display mouse cursor.
        }
        
        #endregion

        #region Methods

       
        /// <summary>
        /// Create library of graphic assets.
        /// </summary>
        /// <returns>Library.</returns>
        protected override AssetLibrary GetAssetLibrary()
        {
            AssetLibrary lib = AssetLibrary.CreateAnEmptyLibrary();             // New asset library.
            X2DAsset A = null;                                                  // Temporary variable used to create graphic assets.

            // Create a new graphic asset  for the aquarium visuals using class X2DAsset.
            A = new X2DAsset("AquariumVisuals", "AquariumBackground"). 
                UVOriginAt(400, 300).
                UVTopLeftCornerAt(0, 0).
                Width(800).
                Height(600); 
            
            // Import aquarium visual asset in the library.
            lib.ImportAsset(A);

            // Create a new graphic asset for the first progress marker visuals using class X2DAsset.
            A = new X2DAsset("ChickenLegVisuals", "ChickenLeg").
                UVOriginAt(64, 64).
                UVTopLeftCornerAt(0, 0).
                Width(128).
                Height(128);

            // Import first marker visual asset in the library
            lib.ImportAsset(A);

            // Create a new graphic asset  for the orange fish visuals using class X2DAsset.
            A = new X2DAsset("OrangeFishVisuals", "OrangeFish").
                UVOriginAt(64, 64).
                UVTopLeftCornerAt(0, 0).
                Width(128).
                Height(84);

            // Import orange fish visual asset in the library
            lib.ImportAsset(A);

            //Imports the first Piranha visual
            A = new X2DAsset("PiranhaVisuals1", "Piranha1").
                UVOriginAt(64, 64).
                UVTopLeftCornerAt(0, 0).
                Width(128).
                Height(128);

            // Import into library
            lib.ImportAsset(A);

            // Imports the second Piranha visual - Didn't work as intended so idea scrapped, left in if alternative found later
            A = new X2DAsset("PiranhaVisuals2", "Piranha2").
                UVOriginAt(64, 64).
                UVTopLeftCornerAt(0, 0).
                Width(128).
                Height(128);

            // Import into library
            lib.ImportAsset(A);

            // Return library.
            return lib;
        }

        /// <summary>
        /// Load contents for the simulation.
        /// LoadContent will be called only once, at the beginning of the simulation,
        /// is the place to load all of your content (e.g. graphics and sounds).
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Instantiate and initialize scene, specifying its horizontal size (800)
            // and vertical size (600).
            // Note, the third parameter is set to 0 because unused in FishORama.
            mScene = XNAGame.CreateA2DScene(800, 600, 0);

            /*
             * Create Tokens
             */
            Console.WriteLine("Welcome to the FishORama Framework");

            AquariumToken aquarium = new AquariumToken("Aquarium", this, 800, 600);         // Create aquarium token.
                                                                                                                                
            /*
             * Place tokens in the scene.
             */

            Vector3 tokenPos;        // Empty Vector3 object to be used to position tokens.

            tokenPos = new Vector3(0, 0, 0);            // Define scene position for the aquarium.
            mScene.Place(aquarium, tokenPos);           // Place token in scene.

            //Nested for loop using an array of Piranhas which assigns each team three piranhas, and sets each of them their own number
            //This then also assigns them either team 1 or team 2
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < piranha.Length; i++)
                {
                    //Where: ("String", aqarium reference, piranha number, team number)
                    piranha[i] = new PiranhaToken("Pirahna", aquarium, i + 1, j + 1);
                    //X and Y are hard coded positions declared inside Data Members
                    piranhaPos = new Vector3(x, y, 1);
                    //Places Piranha into the scene
                    mScene.Place(piranha[i], piranhaPos);
                    //Spaces the piranhas out vertically across the stage so they are not on top of one another
                    y += 150;
                }
                //Moves the Piranhas over to the other side of the scene
                x = 300;
                //Resets the Y coordinate 
                y = -150;
            }

            /*
             * Create and Initialize camera
             */

            // Define the position for the camera as a 3D vector object, created as a new
            // instance of class Vector3, and initialized to (0, 0, 1),
            // which means that in FishORama it is centered on the origin of the world.
            Vector3 camPosition = new Vector3(0, 0, 1);

            // Instantiate and initialize camera, specifying its ID ("Camera 01"
            // in this case), and its position (camPosition in this case).
            mCamera = mScene.CreateCameraAt("Camera 01", camPosition);

            //Startup the visualization, giving the "...and ACTION!" directive.
            this.PlayScene(mScene);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Perform standard update operations.
            base.Update(gameTime);
        }

        #endregion
    }
}
