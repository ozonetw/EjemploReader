using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Emotiv;


namespace FinalEEGReader
{
    class Program
    {
        static EmoEngine engine = EmoEngine.Instance;
        //static EmoState es;
        static Single scoreMedidation, scoreExcitement, scoreEngage;   //Scores despues de la correcion algoritmica de emotiv
        static System.Timers.Timer aTimer = new System.Timers.Timer();

        static void Main(string[] args)
        {
            emoConnection();

            Console.WriteLine("Presiona Q para salir.");
            while (Console.Read() != 'q') ;
            /*
            while (true)
            {
                engine.ProcessEvents(1000);
                engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(engine_EmoStateUpdated);
                Console.WriteLine("meditation: " + scoreMedidation);
                Console.WriteLine("Excitement: " + scoreExcitement);
                Console.WriteLine("Engagement: " + scoreEngage);
                
            }
             * */
        }

        //Conexion al dispositivo/Simulador
        private static void emoConnection()
        {
            engine.EmoEngineConnected += new EmoEngine.EmoEngineConnectedEventHandler(engine_EmoEngineConnected);  //Eventos de conexion de usuario
            engine.EmoEngineDisconnected += new EmoEngine.EmoEngineDisconnectedEventHandler(engine_EmoEngineDisconnected); //Eventos de Desconexión de usuario
            engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(engine_EmoStateUpdated);               //Eventos de actualizacion en deteccion de emociones

            /*
             * Conexiones. Adaptar segun sea el caso.
             */
            //engine.Connect();
            engine.RemoteConnect("127.0.0.1", 1726);

            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(onTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

        }

        static void engine_EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;
            scoreMedidation = es.AffectivGetMeditationScore();
            //Console.WriteLine(scoreMedidation);
            scoreExcitement = es.AffectivGetExcitementShortTermScore();
            //Console.WriteLine(scoreExcitement);
            scoreEngage = es.AffectivGetEngagementBoredomScore();
            //Console.WriteLine("meditation: " + es.AffectivGetMeditationScore());
            //Console.WriteLine("Excitement: " + es.AffectivGetExcitementShortTermScore());
            //Console.WriteLine("Frustration: " + es.AffectivGetFrustrationScore());

        }

        static void engine_EmoEngineDisconnected(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("Disconnected");
        }

        static void engine_EmoEngineConnected(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("Connected");
        }

        static void onTimedEvent(object source, ElapsedEventArgs e)
        {
            engine.ProcessEvents();
            engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(engine_EmoStateUpdated);
            Console.WriteLine("meditation: " + scoreMedidation);
            Console.WriteLine("Excitement: " + scoreExcitement);
            Console.WriteLine("Engagement: " + scoreEngage);

        }
    }
}
