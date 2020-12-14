import API from './dnd_api';

class rollService {
    static rollDice(rollType){
        return API.get('roll/' + rollType.replace('+', 'p').replace('-', 'm'));        
    }
}

export default rollService;