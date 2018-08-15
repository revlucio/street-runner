import React, { Component} from "react";
import {hot} from "react-hot-loader";
import "./App.css";

class App extends Component{
    render(){
        return(
            <div className="App">
                <div className="bar">
                    <div className="something">
                        <h1>Welcome to Street Runner</h1>
                        <p>Want to see how much of London you've covered in your Strava runs?</p>
                        <a href="/api/map/east-london">view your running map!</a>
                    </div>    
                </div>
            </div>
        );
    }
}

export default hot(module)(App);