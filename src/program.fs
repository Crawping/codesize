namespace UI

open System
open System.IO
open System.Windows

type MainWindow() =
    inherit codesize.MainWindow()

module Program =
    [<STAThread>] do ()

    let app = Application(ShutdownMode = ShutdownMode.OnMainWindowClose)

    let settingsPath = sprintf "%s\\codesize\\settings.xml" $ Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

    if File.Exists(settingsPath) then
        try UI.Settings.current.Load settingsPath
        with _ -> ()

    app.Exit.Add(fun _ ->
        try UI.Settings.current.Save settingsPath
        with :? IOException -> ())

    app.DispatcherUnhandledException.Add(fun args ->
        let e = args.Exception
        UI.Exception.uploadException e
        UI.Exception.showModal e
        args.Handled <- true)

    AppDomain.CurrentDomain.UnhandledException.Add(fun args ->
        let e = unbox args.ExceptionObject
        UI.Exception.uploadException e
        UI.Exception.showMessage e)

    app.Exit.Add(fun _ ->
        UI.Exception.uploadWait 2000)

    app.Run(codesize.window) |> ignore