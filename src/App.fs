module Firebase.Fable

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Firebase
open Fable.Import
open Fable.Import

let config =
    createObj [
        "apiKey" ==> ""
        "authDomain" ==> ""
        "databaseURL" ==> ""
        "projectId" ==> ""
        "storageBuckdet" ==> ""
        "messagingSenderId" ==> ""
    ] :?> JS.Object
let app = firebase.initializeApp(config, null) |> ignore
let auth = firebase.auth()


let register email password =
    promise {
        let! user = auth.createUserWithEmailAndPassword(email, password)

        let email =
            match user with
            | Some u ->
                sprintf "%O" u |> Browser.console.log
                u?email |> string |> Some
            | None -> None
        Option.defaultValue "no email" email
        |> Browser.console.log
        return email
    }

let login email password =
    promise {
        let! user = auth.signInWithEmailAndPassword(email, password)
        sprintf "%O" user |> Browser.console.log
        let email =
            match user with
            | Some u -> u?email |> string |> Some
            | None -> None
        Option.defaultValue "no email" email
        |> Browser.console.log
        return email
    }


let getById<'T when 'T :> Browser.HTMLElement> id =
    Browser.document.getElementById(id) :?> 'T

let log s = Browser.console.log s
let testButtonClick _ =
    fetch "https://api.pokemontcg.io/v1/cards?name=pikachu" []
    |> Promise.bind (fun res -> res.text())
    |> Promise.map (fun txt -> txt.Length)
    |> Promise.map(fun l -> Browser.console.log(l))
    |> ignore;
    null;

let handleRegister r =
    match r with
    | Some r -> Browser.console.log(r)
    | None -> Browser.console.error("No response")

let registerClick _ =
    let email = getById<Browser.HTMLInputElement> "username"
    let password = getById<Browser.HTMLInputElement> "password"
    log email.value
    log password.value
    register email.value password.value
    |> Promise.map handleRegister

let loginClick _ =
    let email = getById<Browser.HTMLInputElement> "username"
    let password = getById<Browser.HTMLInputElement> "password"
    log email.value
    log password.value
    login email.value password.value
    |> Promise.map handleRegister

let init() =

    let tButton = getById "test"
    tButton.onclick <- testButtonClick
    let rButton = getById "register"
    rButton.onclick <- registerClick

    let lButton = getById "login"
    lButton.onclick <- loginClick



init()