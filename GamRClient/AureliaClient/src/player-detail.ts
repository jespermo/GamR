import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {Api} from './api';
import {areEqual} from './utility';
import {PlayerUpdated,PlayerViewed} from './messages';

interface Player {
  
  firstName: string;
  lastName: string;
  email: string;
}

@inject(Api,EventAggregator)
export class PlayerDetail {
  routeConfig;
  player: Player;
  originalPlayer: Player;

  constructor(private api: Api, private ea: EventAggregator) { }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;

    return this.api.getPlayerDetails(params.id).then(player => {
      this.player = <Player> player;
      this.routeConfig.navModel.setTitle(this.player.firstName);
      this.originalPlayer = JSON.parse(JSON.stringify(this.player));
      this.ea.publish(new PlayerViewed(this.player));
    });
  }

  get canSave() {
    return this.player.firstName && this.player.lastName && !this.api.isRequesting;
  }

  save() {
    this.api.savePlayer(this.player).then(player => {
      this.player = <Player> player;
      this.routeConfig.navModel.setTitle(this.player.firstName);
      this.originalPlayer = JSON.parse(JSON.stringify(this.player));
      this.ea.publish(new PlayerUpdated(this.player));
    });
  }

  canDeactivate() {
    if(!areEqual(this.originalPlayer, this.player)){
      let result = confirm('You have unsaved changes. Are you sure you wish to leave?');

      if(!result) {
        this.ea.publish(new PlayerViewed(this.player));
      }

      return result;
    }

    return true;
  }
}