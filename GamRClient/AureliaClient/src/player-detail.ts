import {inject} from 'aurelia-framework';
import {Api} from './api';
import {areEqual} from './utility';

interface Player {
  firstName: string;
  lastName: string;
  email: string;
}

@inject(Api)
export class PlayerDetail {
  routeConfig;
  player: Player;
  originalPlayer: Player;

  constructor(private api: Api) { }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;

    return this.api.getPlayerDetails(params.id).then(player => {
      this.player = <Player> player;
      this.routeConfig.navModel.setTitle(this.player.firstName);
      this.originalPlayer = JSON.parse(JSON.stringify(this.player));
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
    });
  }

  canDeactivate() {
    if (!areEqual(this.originalPlayer, this.player)) {
      return confirm('You have unsaved changes. Are you sure you wish to leave?');
    }

    return true;
  }
}