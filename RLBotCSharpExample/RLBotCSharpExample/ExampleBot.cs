using System;
using RLBotDotNet;
using rlbot.flat;

namespace RLBotCSharpExample
{
    // We want to our bot to derive from Bot, and then implement its abstract methods.
    class ExampleBot : Bot
    {
        // We want the constructor for ExampleBot to extend from Bot, but we don't want to add anything to it.
        public ExampleBot(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex) { }

        public override Controller GetOutput(GameTickPacket gameTickPacket)
        {
            // This controller object will be returned at the end of the method.
            // This controller will contain all the inputs that we want the bot to perform.
            Controller controller = new Controller();

            // Store the required data from the gameTickPacket.
            // The GameTickPacket's attributes are nullables, so you must use .Value.
            // It is recommended to create your own internal data structure to avoid the displeasing .Value syntax.
            Vector3 ballLocation = gameTickPacket.Ball.Value.Physics.Value.Location.Value;
            Vector3 carLocation = gameTickPacket.Players(this.index).Value.Physics.Value.Location.Value;
            Rotator carRotation = gameTickPacket.Players(this.index).Value.Physics.Value.Rotation.Value;

            // Calculate to get the angle from the front of the bot's car to the ball.
            double botToTargetAngle = Math.Atan2(ballLocation.Y - carLocation.Y, ballLocation.X - carLocation.X);
            double botFrontToTargetAngle = botToTargetAngle - carRotation.Yaw;
            // Correct the angle
            if (botFrontToTargetAngle < -Math.PI)
                botFrontToTargetAngle += 2 * Math.PI;
            if (botFrontToTargetAngle > Math.PI)
                botFrontToTargetAngle -= 2 * Math.PI;

            //+1 is left steer and -1 is right steer
            if (botFrontToTargetAngle > 0)
                controller.Steer = 1;
            else
                controller.Steer = -1;

            //Variable throttle control
            controller.Throttle = 1;

            return controller;
        }
    }
}
