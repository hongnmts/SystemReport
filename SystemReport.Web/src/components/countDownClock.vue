<template>
<div>
  <div class="container" style="display: flex; justify-content: center; align-items: center; flex-direction: column">
    <section class="bubble-holder">
      <div class="bubble-1 bubble-container bubble-animation-x">
        <div class="bubble-large bubble-animation-y"></div>
      </div>

      <div class="bubble-2 bubble-container bubble-animation-x">
        <div class="bubble-large bubble-animation-y"></div>
      </div>

      <div class="bubble-3 bubble-container bubble-animation-x">
        <div class="bubble-large bubble-animation-y"></div>
      </div>

      <div class="bubble-4 bubble-container bubble-animation-x">
        <div class="bubble-small bubble-animation-y"></div>
      </div>

      <div class="bubble-5 bubble-container bubble-animation-x">
        <div class="bubble-small bubble-animation-y"></div>
      </div>

      <div class="bubble-6 bubble-container bubble-animation-x">
        <div class="bubble-small bubble-animation-y"></div>
      </div>

      <div class="bubble-7 bubble-container bubble-animation-x">
        <div class="bubble-small bubble-animation-y"></div>
      </div>

      <div class="bubble-8 bubble-container bubble-animation-x">
        <div class="bubble-small bubble-animation-y"></div>
      </div>
    </section>
    <div id="timer"></div>
  </div>

</div>
</template>

<script>
export default {
  name: "countDownClock",
  created() {
    this.init();
  },
  methods:{
    init(){
      const year = new Date().getFullYear();
      const fourthOfJuly = new Date(year, 6,4).getTime();
      const fourthOfJulyNextYear = new Date(year + 1, 6, 4).getTime();
      const month = new Date().getMonth();

// countdown
      let mn = 5;
      let sec = 0;
      let timer = setInterval(function() {
        if(sec == 0){
          mn-=1;
          sec = 59;
        }else{
          sec-=1;
        }


        // get today's date
        const today = new Date().getTime();

        // get the difference
        let diff;
        if(month > 6) {
          diff = fourthOfJulyNextYear - today;
        } else {
          diff = fourthOfJuly - today;
        }


        // math
        let minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
        let seconds = Math.floor((diff % (1000 * 60)) / 1000);

        // display
        document.getElementById("timer").innerHTML =
            "<div class=\"minutes\"> \
              <div class=\"numbers\">" + mn + "</div>phút</div> \
<div class=\"seconds\"> \
  <div class=\"numbers\">" + sec + "</div>giây</div> \
</div>";



      }, 1000);
    }
  }
}
</script>

<style lang="scss" scoped>
::v-deep {
  $font: 'Lato', sans-serif;
  $big: 'Montserrat', sans-serif;

  $red: #EF2F3C;
  $white: #F6F4F3;
  $blue: #276FBF;
  $dark: #183059;
  $yellow: #F0A202;

  $star: polygon(50% 0%, 61% 35%, 98% 35%, 68% 57%, 79% 91%, 50% 70%, 21% 91%, 32% 57%, 2% 35%, 39% 35%);




  h1 {
    font-family: $font;
    text-align: center;
    margin-top: 2em;
    font-size: 1em;
    text-transform: uppercase;
    letter-spacing: 5px;
    color: $white;
  }

  #timer {
    color: $white;
    text-align: center;
    text-transform: uppercase;
    font-family: $font;
    font-size: .7em;
    letter-spacing: 5px;
  }

  .days, .hours, .minutes, .seconds {
    display: inline-block;
    padding: 20px;
    width: 100px;
    border-radius: 5px;
  }

  .days {
    background: $red;
  }

  .hours {
    background: $white;
    color: $dark;
  }

  .minutes {
    background: $blue;
  }

  .seconds {
    background: $yellow;
    .numbers {

    }
  }

  .numbers {
    font-family: $big;
    color: white;
    font-size: 4em;
    font-weight: bold;
  }

  .white {
    position: absolute;
    background: $white;
    height: 85px;
    width: 75px;
    left: 30%;
    top: 2%;
    .triangle {
      border-bottom: 14px solid $white;
    }
    .string {
      background: $white;
      border: 1px solid $white;
    }
  }

  .red {
    position: absolute;
    background: $red;
    left: 18%;
    top: 9%;
    height: 65px;
    width: 70px;
    .triangle {
      border-bottom: 14px solid $red;
    }
    .string {
      background: $red;
      border: 1px solid $red;
    }
  }

  .blue {
    position: absolute;
    background: $blue;
    height: 80px;
    width: 80px;
    left: 30%;
    top: 5%;
    .triangle {
      border-bottom: 14px solid $blue;
    }
    .string {
      background: $blue;
      border: 1px solid $blue;
    }
  }

  .balloon {
    border: 1px solid #000;
    border-radius: 50% 50% 50% 50%/ 40% 40% 60% 60%;
    z-index: 2;
  }

  .eye {
    position: absolute;
    width: 7px;
    height: 7px;
    top: 40%;
    left: 22%;
    background: #000;
    border-radius: 50%;
    &:after {
      content: '';
      left: 35px;
      width: 7px;
      height: 7px;
      border-radius: 50%;
      background: #000;
      position: absolute;
    }
  }

  .mouth {
    position:absolute;
    top: 45%;
    left: 43%;
    width: 7px;
    height: 7px;
    border-radius: 50%;
  }

  .happy {
    border: 2px solid;
    border-color: transparent #000 #000 transparent;
    transform: rotate(45deg);
  }

  .triangle {
    position: absolute;
    left: 40%;
    bottom: -10%;
    width: 0;
    height: 0;
    border-left: 7px solid transparent;
    border-right: 7px solid transparent;
  }

  .string {
    position: absolute;
    height: 70px;
    width: 1px;
    left: 48%;
    top: 100%;
    z-index: -1;
  }

  .star {
    width: 20px;
    height: 20px;
    background: $white;
    -webkit-clip-path: $star;
    clip-path: $star;
  }

  .star-red {
    width: 30px;
    height: 30px;
    margin-left: 51px;
    margin-top: -5px;
    background: $red;
    -webkit-clip-path: $star;
    clip-path: $star;
  }

  footer {
    position: fixed;
    bottom: 0;
    right: 0;
    text-transform: uppercase;
    padding: 10px;
    font-family: $font;
    font-size: 0.7em;
    p {
      letter-spacing: 3px;
      color: $red;
    }
    a {
      color: $white;
      text-decoration: none;
      &:hover {
        color: $blue;
      }
    }
  }
}

</style>
<style>
/* Main bubbles */

.bubble-holder{
  width:60%;
  height: 100px;
  position: relative;
  overflow:hidden;
}

.bubble-1 {
  left: 8%;
  animation-delay: 1s;
}
.bubble-1 .bubble-animation-y{
  animation-duration: 7s;
  animation-delay: 0s;
}

.bubble-2 {
  left: 40%;
  animation-delay: 2s;
}
.bubble-2 .bubble-animation-y{
  animation-duration: 6s;
  animation-delay: 1s;
}

.bubble-3 {
  right: 15%;
  animation-delay: 1s;
}
.bubble-3 .bubble-animation-y{
  animation-duration: 5s;
  animation-delay: 2s;
}

.bubble-4 {
  left: 8%;
  animation-delay: 2s;
}
.bubble-4 .bubble-animation-y{
  animation-duration: 6s;
  animation-delay: 1s;
}

.bubble-5 {
  left: 40%;
  animation-delay: 1s;
}
.bubble-5 .bubble-animation-y{
  animation-duration: 5s;
  animation-delay: 0;
}

.bubble-6 {
  left: 45%;
  animation-delay: 2s;
}
.bubble-6 .bubble-animation-y{
  animation-duration: 7s;
  animation-delay: 2s;
}

.bubble-7 {
  right: 15%;
  animation-delay: 1s;
}
.bubble-7 .bubble-animation-y{
  animation-duration: 5.5s;
  animation-delay: 1s;
}

.bubble-8 {
  right: 5%;
  animation-delay: 2s;
}
.bubble-8 .bubble-animation-y{
  animation-duration: 7s;
  animation-delay: 1s;
}


/* Static Circles */

.bubble-container{
  position: absolute;
  bottom: 0px;

}

.bubble-large, .bubble-small {
  border-radius: 100%;
  box-shadow: 0 20px 30px rgba(0, 0, 0, 0.1),
  inset 0px 10px 30px 5px rgba(124, 172, 217,0.5);
  background-image: url("../assets/images/DTHU.png");
  background-size: cover;
}

.bubble-large {
//border: 7px solid #055E8F;
  width: 50px;
  height: 50px;
}

.bubble-small {
//border: 5px solid #055E8F;
  width: 20px;
  height: 20px;
}

.bubble-animation-x {
  animation-duration: 3s;
  animation-iteration-count: infinite;
  animation-name: bubbleXAnimFrames;
  animation-timing-function: ease;
  animation-direction: alternate;
}

.bubble-animation-y {
  animation-iteration-count: infinite;
  animation-name: bubbleYAnimFrames;
  animation-timing-function: linear;
}


@keyframes bubbleXAnimFrames {
  0% {
    transform: translateX(0);
  }
  100% {
    transform: translateX(50px);
  }
}

@keyframes bubbleYAnimFrames {
  0% {
    transform: translateY(0);
  }
  50% {
    opacity: 85%;
  }
  100% {
    transform: translateY(-150px);
    opacity: 0;
  }
}
</style>